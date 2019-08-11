using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class Colour32Uniform
    {
        private readonly int UniformLocation;
        private OpenTK.Graphics.Color4 data;

        internal Colour32Uniform(int location)
        {
            UniformLocation = location;
        }

        internal OpenTK.Graphics.Color4 Data
        {
            get { return data; }
            set { data = value; }
        }

        internal void TransferData()
        {
            GL.Uniform4(UniformLocation, data);
        }
    }

}
