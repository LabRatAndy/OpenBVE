using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class Vector4Uniform
    {
        private readonly int Location;
        private Vector4 data;

        internal Vector4Uniform(int location)
        {
            Location = location;
        }

        internal Vector4 Data
        {
            get { return data; }
            set { data = value; }
        }

        internal void TransferData()
        {
            GL.Uniform4(Location, ref data);
        }
    }

}