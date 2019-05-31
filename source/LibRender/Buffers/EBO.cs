using System;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    public class ElementBufferObject : IDisposable
    {
        /// <summary>
        /// openGL handle for the EBO
        /// </summary>
        private int handle;

        /// <summary>
        /// EBO data as an array of ints that represent the index of the vertex in the VBO
        /// </summary>
        private int[] ibo = null;

        /// <summary>
        /// Constructor using the supplied array of int representing the index of the vertex in the VBO
        /// </summary>
        /// <param name="IBO">Array of ints containing the vertex indices for the EBO</param>
        public ElementBufferObject(int[] IBO)
        {
            GL.GenBuffers(1, out handle);
            ibo = IBO;
        }

        /// <summary>
        /// Binds/activates the EBO for use
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
        }

        /// <summary>
        /// Buffers the indices in the EBO to OpenGL ready for use, must be done before drawing
        /// </summary>
        /// <param name="drawtype">Hint to tell openGL about how likely the data is to change ie the object is to be moved</param>
        public void BufferData(BufferUsageHint drawtype)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ibo.Length * sizeof(int), ibo, drawtype);
        }

        /// <summary>
        /// Draws the ibo
        /// </summary>
        /// <param name="primitiveType"> the type of openGL primitive that the vertices are arranged in</param>
        public void Draw(PrimitiveType primitiveType)
        {
            GL.DrawElements(primitiveType, ibo.Length, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>
        /// Unbinds/deactivates the EBO from use
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Cleans up deleting the openGL EBO
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliser
        /// </summary>
        ~ElementBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }

}
