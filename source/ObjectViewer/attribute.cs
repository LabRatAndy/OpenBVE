using OpenTK.Graphics.OpenGL;

namespace OpenBve
{
    /// <summary>
    /// class to represent an attribute stored by the VAO class
    /// </summary>
    internal class Attribute
    {
        private string name;
        private int size;
        private int offset;
        private int stride;
        private int index;
        private VertexAttribPointerType type;
        private bool normalised;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Name">The name of the attribute in the shader</param>
        /// <param name="Size">The number of elements in the attribute eg 3 for a 3d coordinate</param>
        /// <param name="Offset">The number of bytes the attribute is from the start of the vertex's data segment</param>
        /// <param name="Stride">The number of bytes between each of these attributes in the array. ie total length of each vertex's data segment</param>
        /// <param name="Index">The index of the attribute in the layout for the shader</param>
        /// <param name="Type">OpenTK's enumeration of the type of data eg float</param>
        /// <param name="Normalised">Is the data normalised, usually false</param>
        internal Attribute(string Name, int Size,int Offset, int Stride, int Index, VertexAttribPointerType Type, bool Normalised)
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
        /// sets the attribute ready for use in the VAO
        /// </summary>
        internal void SetAttribute()
        {
            GL.VertexAttribPointer(index, size, type, normalised, stride, offset);
        }
        /// <summary>
        /// Enables the attribute
        /// </summary>
        internal void EnableAttribute()
        {
            GL.EnableVertexAttribArray(index);
        }
        /// <summary>
        /// disables the attribute
        /// </summary>
        internal void DisableAttribute()
        {
            GL.DisableVertexAttribArray(index);
        }
    }


} 