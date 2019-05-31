using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    public class Attribute
    {
        private string name;
        private int size;
        private int offset;
        private int stride;
        private int index;
        private VertexAttribPointerType type;
        private bool normalised;

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

        public void SetAttribute()
        {
            GL.VertexAttribPointer(index, size, type, normalised, stride, offset);
        }

        public void EnableAttribute()
        {
            GL.EnableVertexAttribArray(index);
        }

        public void DisableAttribute()
        {
            GL.DisableVertexAttribArray(index);
        }
    }
}