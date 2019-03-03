﻿using System;
using OpenTK;

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
    internal struct AssemblerVertex
    {
        internal OpenBveApi.Math.Vector3 coordinate;
        internal OpenBveApi.Math.Vector3 Normal;
        internal OpenBveApi.Math.Vector2 texture;
    }


}