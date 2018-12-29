using System;
using OpenTK.Graphics.OpenGL;


namespace OpenBve
{
    /// <summary>
    /// Class representing an openTK/openGL EBO/IBO
    /// </summary>
    internal class ElementBufferObject : IDisposable
    {
        private int handle;
        private int[] ibo = null;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ibo"> An int array of vertex indices that make the object</param>
        internal ElementBufferObject(int[] ibo)
        {
            GL.GenBuffers(1, out handle);
            this.ibo = ibo;
        }
        /// <summary>
        /// Binds the EBO ready for use
        /// </summary>
        internal void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
        }
        /// <summary>
        /// Unbinds the EBO Deactivating it
        /// </summary>
        internal void UnBind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        /// <summary>
        /// Copies the Indices in to the EBO must be done before use
        /// </summary>
        /// <param name="drawtype">Hint which tells openGL how the object is likely to be used</param>
        internal void BufferData(BufferUsageHint drawtype)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ibo.Length * sizeof(int), ibo, drawtype);
        }
        /// <summary>
        /// Draws the object
        /// </summary>
        internal void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, ibo.Length, DrawElementsType.UnsignedInt, 0);
        }
        /// <summary>
        /// Cleans up relaseing the openGL EBO
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finalizer, Call Dispose not this 
        /// </summary>
        ~ElementBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }
}
