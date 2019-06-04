using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    /// <summary>
    /// Class representing an openGL attribute as used by the VAO / VBO representing how the vertex data is stored in the VBO
    /// </summary>
    public class Attribute
    {
        private string name;
        private int size;
        private int offset;
        private int stride;
        private int index;
        private VertexAttribPointerType type;
        private bool normalised;

        /// <summary>
        /// Constructor creates the attribute object
        /// </summary>
        /// <param name="Name">The attribute name as used in the shader, although strictly speaking not passed to opengl is useful for readablity</param>
        /// <param name="Size">The number of data points in the attribute ie 3 for XYZ coordinates</param>
        /// <param name="Offset">The number of bytes that the attribute is from the start of the vertex data element in it's enitrity for first attribute in the vertex data element it is 0 </param>
        /// <param name="Stride">The total size in bytes of the entire vertex data element </param>
        /// <param name="Index">The index of the attribute as it is in the shader </param>
        /// <param name="Type">OpenTK enum indicating the data type that forms the attribute</param>
        /// <param name="Normalised">Does the data need normalising; default = false</param>
        public Attribute(string Name, int Size, int Offset, int Stride, int Index, VertexAttribPointerType Type, bool Normalised = false)
        {
            name = Name;
            size = Size;
            offset = Offset;
            stride = Stride;
            index = Index;
            type = Type;
            normalised = Normalised;
        }

        /// <summary>
        /// Sets the attribute in openGL
        /// </summary>
        public void SetAttribute()
        {
            GL.VertexAttribPointer(index, size, type, normalised, stride, offset);
        }

        /// <summary>
        /// Enables the attribute for use in openGL
        /// </summary>
        public void EnableAttribute()
        {
            GL.EnableVertexAttribArray(index);
        }

        /// <summary>
        /// Disables the attribute from use in openGL
        /// </summary>
        public void DisableAttribute()
        {
            GL.DisableVertexAttribArray(index);
        }
    }
}