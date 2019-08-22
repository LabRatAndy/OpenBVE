using System;
using OpenBveApi.LibRender;

namespace OpenBveApi.Objects
{
    /// <summary>
    /// Represents a mesh that can be used by a shader made up of a vbo to hold the vertices and EBO to hold the faces
    /// </summary>
    public struct ShaderMesh
    {
        /// <summary>
        /// VBO to hold the vertices
        /// </summary>
        private VertexBufferObject vertices;
        /// <summary>
        /// EBO to hold face data
        /// </summary>
        private ElementBufferObject[] faces;
        /// <summary>
        /// materials used in the mesh
        /// </summary>
        private MeshMaterial[] materials;
        /// <summary>
        /// Creates a shadermesh struct
        /// </summary>
        /// <param name="vbo">VBO containing the vertex data</param>
        /// <param name="ebos">Array of EBOs containg the face data</param>
        /// <param name="materials">Array of meshmaterial structs containing materials used in the mesh</param>
        public ShaderMesh(VertexBufferObject vbo, ElementBufferObject[] ebos, MeshMaterial[] materials)
        {
            vertices = vbo;
            faces = ebos;
            this.materials = materials;
        }
        /// <summary>
        /// Gets the VBO containing the vertex data
        /// </summary>
        public VertexBufferObject Vertices
        {
            get { return vertices; }
        }
        /// <summary>
        /// Gets the array of EBOs containing face data
        /// </summary>
        public ElementBufferObject[] Faces
        {
            get { return faces; }
        }
        /// <summary>
        /// Gets the array of MeshMaterial structs containing the materials used in the mesh
        /// </summary>
        public MeshMaterial[] Materials
        {
            get { return materials; }
        }
        /// <summary>
        /// Cleans up the shader mesh releasing openGL resources.
        /// </summary>
        public void Dispose()
        {
            //ebos
            for (int n = 0; n < faces.Length; n++)
            {
                faces[n].Dispose();
            }
            //vbo
            vertices.Dispose();
        }


    }
}