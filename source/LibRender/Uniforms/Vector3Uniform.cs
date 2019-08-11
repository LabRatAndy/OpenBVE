using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    internal class Vector3Uniform
    {
        private readonly int Location;
        private Vector3 data;

        internal Vector3Uniform(int location)
        {
            Location = location;
        }

        internal Vector3 Data
        {
            get { return data; }
            set { data = value; }
        }

        internal void TransferData()
        {
            GL.Uniform3(Location, ref data);
        }

    }

}