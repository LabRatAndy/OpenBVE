using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class Colour24Uniform
    {
        private readonly int UniformLocation;
        private Vector3 data;

        internal Colour24Uniform(int location)
        {
            UniformLocation = location;
        }

        internal Vector3 Data
        {
            get { return data; }
            set { data = value; }
        }

        internal void TransferData()
        {
            GL.Uniform3(UniformLocation, ref data);
        }
    }
}