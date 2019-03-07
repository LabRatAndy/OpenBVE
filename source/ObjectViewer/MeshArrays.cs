using System;
using OpenTK;
using System.Collections.Generic;

namespace OpenBve
{
    internal struct MeshArrays
    {
        VertexArrayObject vao;
        VertexBufferObject vertices;
        List<ElementBufferObject> ibo;
        ushort[] materials;
        internal VertexArrayObject VAO
        {
            get { return vao; }
            set { vao = value; }
        }
        internal VertexBufferObject VBO
        {
            get { return vertices; }
            set { vertices = value; }
        }
        internal List<ElementBufferObject> EBOS
        {
            get { return ibo; }
            set { ibo = value; }
        }
        internal ushort[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }
        /// <summary>
        /// Adds an ebo to the ebo list(face)
        /// </summary>
        /// <param name="ebo"> the EBO object to add</param>
        /// <returns>the index in the list</returns>
        internal int AddEbo(ElementBufferObject ebo)
        {
            if (ibo == null) ibo = new List<ElementBufferObject>();
            ibo.Add(ebo);
            return ibo.IndexOf(ebo);
        }
        /// <summary>
        /// returns the ebo at the given index
        /// </summary>
        /// <param name="index">index of ebo to retrieve</param>
        /// <returns>The EBO object</returns>
        internal ElementBufferObject GetEBO(int index)
        {
            return ibo[index];
        }

    }
    internal struct AssemblerVertex
    {
        internal OpenBveApi.Math.Vector3 coordinate;
        internal OpenBveApi.Math.Vector3 Normal;
        internal OpenBveApi.Math.Vector2 texture;
    }


}
