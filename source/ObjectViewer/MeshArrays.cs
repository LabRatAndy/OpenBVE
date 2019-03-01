using System;


namespace OpenBve
{
    internal struct MeshArrays
    {
        VertexArrayObject vao;
        VertexBufferObject vertices;
        ElementBufferObject ibo;
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
        internal ElementBufferObject EBO
        {
            get { return ibo; }
            set { ibo = value; }
        }

    }

}
