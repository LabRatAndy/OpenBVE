using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class TextureUniform
    {
        private readonly int UniformLocation;
        private int data;

        internal TextureUniform(int location)
        {
            UniformLocation = location;
        }

        internal int Data
        {
            get { return data; }
            set { data = value; }
        }

        internal void TransferData()
        {
            GL.Uniform1(UniformLocation, data);
        }
    }

}