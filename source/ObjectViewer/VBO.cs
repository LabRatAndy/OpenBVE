using System;
using OpenTK.Graphics.OpenGL;

namespace OpenBve
{
    /// <summary>
    /// class that represents an openGL/openTK vertex buffer object 
    /// </summary>
    internal class VertexBufferObject : IDisposable
    {
        private int handle;
        private float[] vertexData = null; //need to check the type or possibly the vertex struct

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="vertexdata">an array containing the actual vertex coordinates and texturecoordinates as X,Y,Z,U,V</param>
        internal VertexBufferObject(float[] vertexdata)
        {
            GL.GenBuffers(1, out handle);
            vertexData = vertexdata;
        }
        /// <summary>
        /// binds the VBO ready for use
        /// </summary>
        internal void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }
        /// <summary>
        /// Unbinds the VBO deactivating the VBO from use
        /// </summary>
        internal void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// Copies the VertexData into the the VBO must be done before using the VBO
        /// </summary>
        /// <param name="drawtype">The hint reperesenting how the object is to be used and therefore guides openGL's optimisation of the object</param>
        internal void BufferData(BufferUsageHint drawtype)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, drawtype);
        }
        /// <summary>
        /// Draws the object, VBO must be bound first
        /// </summary>
        /// <param name="first">The vertex to start drawing from</param>
        /// <param name="count"> The number of  verticies to draw</param>
        internal void Draw(int first,int count)
        {
            GL.DrawArrays(PrimitiveType.Triangles, first, count);
        }
        /// <summary>
        /// Disose method to clean up the VBO releases the openGL Buffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// finalizer, Dispose method should be called not this 
        /// </summary>
        ~VertexBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }
}