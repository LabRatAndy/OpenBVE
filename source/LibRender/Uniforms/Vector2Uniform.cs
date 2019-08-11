using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class Vector2Uniform
    {
        private readonly int Location;
        private Vector2 data;

        internal Vector2Uniform(int location)
        {
            Location = location;
        }

        internal Vector2 Data
        {
            get { return data; }
            set { data = value; }
        }
        internal void TransferData()
        {
            GL.Uniform2(Location, ref data);
        }
    }
}