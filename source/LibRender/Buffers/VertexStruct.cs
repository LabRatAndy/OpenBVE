using System;
using System.Runtime.InteropServices;
using OpenBveApi.Math;

namespace LibRender
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        Vector3 coordinates;
        Vector3 normals;
        Vector2 texcoords;

        public Vertex(Vector3 Coordinates, Vector3 Normals, Vector2 TexCoords)
        {
            this.coordinates = Coordinates;
            this.normals = Normals;
            this.texcoords = TexCoords;
        }
         public Vector3 Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
        public Vector3 Normals
        {
            get { return normals; }
            set { normals = value; }
        }
        public Vector2 TexCoords
        {
            get { return texcoords; }
            set { texcoords = value; }
        }

        public static bool operator ==(Vertex a, Vertex b)
        {
            if (a.coordinates != b.coordinates) return false;
            if (a.normals != b.normals) return false;
            if (a.texcoords != b.texcoords) return false;
            return true;
        }
        public static bool operator !=(Vertex a, Vertex b)
        {
            if (a.coordinates != b.coordinates) return true;
            if (a.normals != b.normals) return true;
            if (a.texcoords != b.texcoords) return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Vertex)) return false;
            return this.Equals((Vertex)obj);
        }

        private bool Equals(Vertex b)
        {
            if (this.coordinates != b.coordinates) return false;
            if (this.normals != b.normals) return false;
            if (this.texcoords != b.texcoords) return false;
            return true;
        }

    }
}