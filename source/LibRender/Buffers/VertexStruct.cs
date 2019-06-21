using System;
using System.Runtime.InteropServices;
using OpenBveApi.Math;

namespace LibRender
{
    /// <summary>
    /// Struct to hold vertex data to asssemble a VBO from 
    /// struct layout sequential tells compiler to make the data sequentially organised as required by a VBO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        /// <summary>
        /// The XYZ coordinates of the vertex
        /// </summary>
        Vector3 coordinates;
        /// <summary>
        /// The vertex normal for the coordinate
        /// </summary>
        Vector3 normals;
        /// <summary>
        /// The UV texture coordinates for the texture
        /// </summary>
        Vector2 texcoords;
        /// <summary>
        /// Constructor for vertex 
        /// </summary>
        /// <param name="Coordinates">Vector3 containing the XYZ coordinates of the vertex</param>
        /// <param name="Normals">Vector3 containing the normal for the Vertex</param>
        /// <param name="TexCoords">Vector 2 containing the texture coordinates for the vertex</param>
        public Vertex(Vector3 Coordinates, Vector3 Normals, Vector2 TexCoords)
        {
            this.coordinates = Coordinates;
            this.normals = Normals;
            this.texcoords = TexCoords;
        }
        /// <summary>
        /// Sets/Gets the Coordinates Vector3
        /// </summary>
         public Vector3 Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
        /// <summary>
        /// Sets/Gets the Normal Vector3 for the Vertex
        /// </summary>
        public Vector3 Normals
        {
            get { return normals; }
            set { normals = value; }
        }
        /// <summary>
        /// Sets/Gets the Texture Coordinates for the Vertex
        /// </summary>
        public Vector2 TexCoords
        {
            get { return texcoords; }
            set { texcoords = value; }
        }
        /// <summary>
        ///  Equality operator for Vertex struct
        /// </summary>
        /// <param name="a">First vertex to compare</param>
        /// <param name="b">Second Vertex to compare </param>
        /// <returns>True if they are the equal false if not</returns>
        public static bool operator ==(Vertex a, Vertex b)
        {
            if (a.coordinates != b.coordinates) return false;
            if (a.normals != b.normals) return false;
            if (a.texcoords != b.texcoords) return false;
            return true;
        }
        /// <summary>
        /// Inequality operator for the Vertex struct
        /// </summary>
        /// <param name="a">First vertex to compare</param>
        /// <param name="b">Second vertex to compare</param>
        /// <returns>True if they are different, false if they are the same</returns>
        public static bool operator !=(Vertex a, Vertex b)
        {
            if (a.coordinates != b.coordinates) return true;
            if (a.normals != b.normals) return true;
            if (a.texcoords != b.texcoords) return true;
            return false;
        }
        /// <summary>
        /// Equals method compared the parameter with the Vertex
        /// </summary>
        /// <param name="obj">object to compare vertex with</param>
        /// <returns>True if the object is the same as the vertex, false if they are different</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vertex)) return false;
            return this.Equals((Vertex)obj);
        }
        /// <summary>
        /// Equals method compares the supplied vertex with the vertex
        /// </summary>
        /// <param name="b">Vertex to compare with the vertex</param>
        /// <returns>True if the vertices match, false if they are differnt</returns>
        private bool Equals(Vertex b)
        {
            if (this.coordinates != b.coordinates) return false;
            if (this.normals != b.normals) return false;
            if (this.texcoords != b.texcoords) return false;
            return true;
        }
        /// <summary>
        /// Gets the Hash code for the vertex
        /// </summary>
        /// <returns>An int representing the hashcode of the vertex</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = this.coordinates.GetHashCode();
                hashcode = (hashcode * 397) ^ this.Normals.GetHashCode();
                hashcode = (hashcode * 397) ^ this.texcoords.GetHashCode();
                return hashcode;
            }
        }

    }
}