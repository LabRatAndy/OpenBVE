using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace LibRender
{
    /// <summary>
    /// Class representing a Matrix4d uniform 
    /// </summary>
    internal class Matrix4dUniform
    {
        /// <summary> the location of the uniform in the shader</summary>
        private readonly int UniformLocation;
        /// <summary>The matrix4d to be transfered between shader and main program</summary>
        private Matrix4d data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">The uniforms location as returned by the Shader class' GetUniformLocation function</param>
        internal Matrix4dUniform (int location)
        {
            UniformLocation = location;
        }
        
        /// <summary>
        /// Gets / Sets the Matrix to be transfered between the shader. TransferData needs to be called to send/receive the data.
        /// </summary>
        internal Matrix4d Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// Called to send and receive data via the uniform link
        /// </summary>
        internal void TransferData()
        {
            GL.UniformMatrix4(UniformLocation, false, ref data);
        }
    }
}
