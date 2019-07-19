using OpenBveApi.Textures;
using OpenBveApi.Objects;
using OpenBveApi.Hosts;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace LibRender
{
    public static partial class Renderer
    {
        static Renderer()
        {
            shaderList = new Shader[10];
            ShadersLoaded = new bool[10];
            for (int n = 0; n < 10; n++)
            {
                shaderList[n] = null;
                ShadersLoaded[n] = false;
            }
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
            if (ShadersLoaded[0] && ShadersLoaded[1] && ShadersLoaded[2] && ShadersLoaded[3] && ShadersLoaded[4])
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
            initialised = true;
        }

        public static double FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }

        public static void UpdateFrame()
        {

        }
    }

}
