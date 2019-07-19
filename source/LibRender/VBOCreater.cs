using System;
using OpenBveApi.Colors;
using OpenBveApi.Objects;
using OpenBveApi.Hosts;
using OpenBveApi.Math;
using OpenBveApi.LibRender;

namespace LibRender
{
    public class VBOCreator
    {
        private HostInterface currenthost;

        private LibRenderVertex[] verticies;
        private int[][] faces;
        private int[] materials;

        public VBOCreator(HostInterface CurrentHost)
        {
            currenthost = CurrentHost;
        }

        public void CreateVBO(StaticObject Object, out VertexBufferObject vbo)
        {
            verticies = new LibRenderVertex[Object.Mesh.Vertices.Length];
            GetVertexData(Object);
            GetFacesandMaterials(Object);
            GetNormals(Object);
            vbo = new VertexBufferObject(GetVertexDataAsDoubleArray());
        }
        public int GetEBOs(out ElementBufferObject[] ebos)
        {
            ElementBufferObject[] ibos = new ElementBufferObject[faces.GetLength(0)];
            for (int n = 0; n < faces.GetLength(0); n++)
            {
                ElementBufferObject ibo = new ElementBufferObject(faces[n]);
                ibos[n] = ibo;
            }
            ebos = ibos;
            return ebos.Length;
        }

        private void GetVertexData(StaticObject obj)
        {
            for (int n = 0; n < obj.Mesh.Vertices.Length; n++)
            {
                GetCoordinates(obj.Mesh.Vertices[n], n);
                GetTextureCoordinates(obj.Mesh.Vertices[n], n);
            }
        }
        private void GetCoordinates(VertexTemplate vert, int pos)
        {
            verticies[pos].Coordinates = new Vector3(vert.Coordinates);
        }
        private void GetTextureCoordinates(VertexTemplate vert, int pos)
        {
            verticies[pos].TexCoords = new Vector2(vert.TextureCoordinates);
        }
        private void GetFacesandMaterials(StaticObject obj)
        {
            // initialise the the faces and materials array
            faces = new int[obj.Mesh.Faces.Length][];
            materials = new int[obj.Mesh.Faces.Length];
            for (int n = 0; n < obj.Mesh.Faces.Length; n++)
            {
                materials[n] = obj.Mesh.Faces[n].Material;
                GetFace(obj.Mesh.Faces[n], n);
            }
        }
        private void GetFace(MeshFace face, int pos)
        {
            faces[pos] = new int[face.Vertices.Length];
            for(int n=0; n<face.Vertices.Length;n++)
            {
                faces[pos][n] = face.Vertices[n].Index;
            }
        }
        private void GetNormals(StaticObject obj)
        {
            for (int face = 0; face < obj.Mesh.Faces.Length; face++)
            {
                for (int vertex = 0; vertex < obj.Mesh.Faces[face].Vertices.Length; vertex++)
                {
                    verticies[obj.Mesh.Faces[face].Vertices[vertex].Index].Normals = new Vector3(obj.Mesh.Faces[face].Vertices[vertex].Normal);
                }
            }
        }
        private double[] GetVertexDataAsDoubleArray()
        {
            int arraylength = verticies.Length * 8;
            double[] vertexdata = new double[arraylength];
            for (int n = 0; n < verticies.Length; n++)
            {
                int vertexoffset = n * 8;
                vertexdata[vertexoffset] = verticies[n].Coordinates.X;
                vertexdata[vertexoffset + 1] = verticies[n].Coordinates.Y;
                vertexdata[vertexoffset + 2] = verticies[n].Coordinates.Z;
                vertexdata[vertexoffset + 3] = verticies[n].Normals.X;
                vertexdata[vertexoffset + 4] = verticies[n].Normals.Y;
                vertexdata[vertexoffset + 5] = verticies[n].Normals.Z;
                vertexdata[vertexoffset + 6] = verticies[n].TexCoords.X;
                vertexdata[vertexoffset + 7] = verticies[n].TexCoords.Y;
            }
            return vertexdata;
        }
    }

}
