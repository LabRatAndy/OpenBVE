using OpenBveApi.Textures;
using OpenBveApi.Objects;
using OpenBveApi.Hosts;
using OpenBveApi.LibRender;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Collections.Generic;

namespace LibRender
{
    public static partial class Renderer
    {
        static Renderer()
        {
            shaderList = new Shader[10];
            ShadersLoaded = new bool[10];
            Objs = new ShaderMesh[10];
            objcount = 0;
            for (int n = 0; n < 10; n++)
            {
                shaderList[n] = null;
                ShadersLoaded[n] = false;
            }
            //create the default FOV and clip distances
            fieldOfView = MathHelper.DegreesToRadians(100.0d);
            nearDrawDistance = 0.001d; // 1 mm
            farDrawDistance = 600.00d; // 600 metres
        }

        public static void LoadShader(ShaderTypeEnum shadertype, string vertexfilename, string fragmentfilename)
        {
            try
            {
                shaderList[(int)shadertype] = new Shader(vertexfilename, fragmentfilename);
            }
            catch(ShaderCompileException e)
            {
                currentHost.ReportProblem(ProblemType.UnexpectedException, e.Message);
                currentHost.ReportProblem(ProblemType.PathNotFound, e.ShaderFileaname);
                ShadersLoaded[(int)shadertype] = false;
                return;
            }
            ShadersLoaded[(int)shadertype] = true;
        }

        public static void InitialiseRenderer()
        {
            // check shaders have been loaded 0 to 4 mandatory will throw exception and refuse to run
            if (ShadersLoaded[0] != true || ShadersLoaded[1] != true || ShadersLoaded[2] != true || ShadersLoaded[3] != true || ShadersLoaded[4] != true)
            {
                currentHost.ReportProblem(ProblemType.UnexpectedException, "Shaders not loaded cannot initalise renderer");
                initialised = false;
                return;
            }
            //setup the attributes for the VAOs
            List<Attribute> attributeList = new List<Attribute>();
            Attribute Coordinates = new Attribute("Coordinates", 3, 0, 8 * sizeof(double), 0, VertexAttribPointerType.Double);
            Attribute Normal = new Attribute("Normal", 3, 3 * sizeof(double), 8 * sizeof(double), 1, VertexAttribPointerType.Double);
            Attribute TexCoords = new Attribute("TexCoords", 2, 6 * sizeof(double), 8 * sizeof(double), 2, VertexAttribPointerType.Double);
            attributeList.Add(Coordinates);
            attributeList.Add(Normal);
            attributeList.Add(TexCoords);
            //create VAO
            VAO = new VertexArrayObject(attributeList);
            VAO.Bind();
            VAO.SetAttributes();
            VAO.UnBind();
            // create projection matrix 
			//TODO aspect ratio not set when running initialise ? when is screen set up
            //ProjectionTransform = Matrix4d.CreatePerspectiveFieldOfView(fieldOfView, Screen.AspectRatio, nearDrawDistance, farDrawDistance);
            //get view matrix
            ViewTransform = Camera.GetViewMatrix();
            initialised = true;
			GL.ClearColor(OpenTK.Graphics.Color4.DarkGreen);
        }

        public static double FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }

        public static double NearDrawDistance
        {
            get { return nearDrawDistance; }
            set { nearDrawDistance = value; }
        }
        public static double FarDrawDistance
        {
            get { return farDrawDistance; }
            set { farDrawDistance = value; }
        }


        public static void UpdateFrame()
        {
            ViewTransform = Camera.GetViewMatrix();
            ProjectionTransform = Matrix4d.CreatePerspectiveFieldOfView(fieldOfView, Screen.AspectRatio, nearDrawDistance, farDrawDistance);
        }

        public static string GetDefaultShader(int shader)
        {
            // vertex shader == 0, fragment shader == 1
            if (shader == 0)
            {
                string shadersource = "";
                return shadersource;
            }
            if (shader == 1)
            {
                string shadersource = "";
                return shadersource;
            }
            return string.Empty;
        }
        public static void RenderObject()
        {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);
            // check initialisation is sucessful
            if (initialised == false) return;
            // check objects to render
            if (objcount == 0) return;
            VAO.Bind();
            for (int n = 0; n < objcount; n++)
            {
                RenderShaderMesh(Objs[n]);
            }
            VAO.UnBind();
        }
        public static void AddObject(ShaderMesh obj)
        {
            Objs[objcount] = obj;
            if (objcount > Objs.Length)
            {
                System.Array.Resize<ShaderMesh>(ref Objs, objcount++);
            }
            else objcount++;           
        }

        private static void RenderShaderMesh(ShaderMesh mesh)
        {
            mesh.Vertices.Bind();
            for (int n = 0; n < mesh.Faces.Length; n++)
            {
                if(mesh.Faces[n].Type == (int)EBOFaceType.ColouredFace)
                {
                    RenderColouredFace(mesh.Faces[n], mesh.Materials[n]);
                }
                else if(mesh.Faces[n].Type == (int)EBOFaceType.TexturedFace)
                {

                }
                else if(mesh.Faces[n].Type == (int)EBOFaceType.Transparent)
                {

                }
            }
            mesh.Vertices.UnBind();
        }

        private static void RenderColouredFace(ElementBufferObject face, MeshMaterial material)
        {
            Shader shader = shaderList[(int)ShaderTypeEnum.ColouredFaceShader];
			shader.Use();
			Matrix4dUniform modelUniform = new Matrix4dUniform(shader.GetUniformHandle("model"));
			Matrix4dUniform viewUniform = new Matrix4dUniform(shader.GetUniformHandle("view"));
			Matrix4dUniform projectionUniform = new Matrix4dUniform(shader.GetUniformHandle("projection")); 
            Colour32Uniform facecolouruniform = new Colour32Uniform(shader.GetUniformHandle("faceColour"));
            facecolouruniform.Data = new OpenTK.Graphics.Color4(material.Color.R, material.Color.G, material.Color.B, material.Color.A);
            facecolouruniform.TransferData();
			modelUniform.Data = modelTransform;
			viewUniform.Data = ViewTransform;
			projectionUniform.Data = ProjectionTransform;
			modelUniform.TransferData();
			viewUniform.TransferData();
			projectionUniform.TransferData();
            face.Bind();
            face.Draw(PrimitiveType.Triangles);
            face.Unbind();
        }
        /// <summary>
        /// Cleanup the openGL resources shaders vbos etc.
        /// </summary>
        public static void Cleanup()
        {
            for (int n = 0; n < objcount; n++)
            {
                Objs[n].Dispose();
            }
            for (int n = 0; n < 10; n++)
            {
               if(shaderList[n] != null) shaderList[n].Dispose();
            }
			if (VAO != null) VAO.Dispose();
        }
    }

}
