using System;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    public class VertexBufferObject : IDisposable
    {
        /// <summary>
        /// OpenGL handle to the VBO object
        /// </summary>
        private int handle;
        /// <summary>
        /// vertex data array of doubles. May need changing to vertex struct array in future. 
        /// </summary>
        private double[] vertexData = null;
        private Vertex[] vertices = null;
        /// <summary>
        /// Constructor for the VBO. Using the supplied array of doubles containing data on the vertices.
        /// </summary>
        /// <param name="vertexdata">array of doubles contianing data on the vertices</param>
        public VertexBufferObject(double[] vertexdata)
        {
            GL.GenBuffers(1, out handle);
            vertexData = vertexdata;
        }
        /// <summary>
        /// Constructor for the VBO using the supplied array of Vertex structs 
        /// </summary>
        /// <param name="vertexdata">Array of vertex Structs containing vertex data</param>
        public VertexBufferObject(Vertex[] vertexdata)
        {
            GL.GenBuffers(1, out handle);
            vertices = vertexdata;
        }
        /// <summary>
        /// Binds / activates the VBO for use
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }
        /// <summary>
        /// Buffers the vertex data to openGL ready for use must be done before the vbo's data can be drawn.
        /// </summary>
        /// <param name="hint">Hint to tell openGL about how likely the data is to change ie the object is to be moved</param>
        public void BufferData(BufferUsageHint hint)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(double), vertexData, hint);
        }
        /// <summary>
        /// Draws the VBO's data using shader that is in use at the time.
        /// </summary>
        /// <param name="primitiveType">The openGL primitive type the vertex data represents</param>
        /// <param name="first">Vertex that is the first to be drawn in the array</param>
        /// <param name="count">The number of vertices to be drawn from the array</param>
        public void Draw(PrimitiveType primitiveType, int first, int count)
        {
            GL.DrawArrays(primitiveType, first, count);
        }
        /// <summary>
        /// Unbinds / deactivates the vbo from use
        /// </summary>
        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// Cleans up by deleting the openGL VBO
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finaliser
        /// </summary>
        ~VertexBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }
}
