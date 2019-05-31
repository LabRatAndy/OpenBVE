using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    public class VertexArrayObject : IDisposable
    {
        private int handle;
        private List<Attribute> attributeList = null;
        private VertexBufferObject vbo = null;
        private List<ElementBufferObject> iboList = null;

        public VertexArrayObject(List<Attribute>Attributes, List<ElementBufferObject> ibos, VertexBufferObject VBO)
        {
            GL.GenVertexArrays(1, out handle);
            attributeList = Attributes;
            iboList = ibos;
            vbo = VBO;
        }

        public void Bind()
        {
            GL.BindVertexArray(handle);
            vbo.Bind();
            foreach(ElementBufferObject ibo in iboList)
            {
                ibo.Bind();
            }
        }

        public void SetAttributes()
        {
            foreach(Attribute attrib in attributeList)
            {
                attrib.SetAttribute();
                attrib.EnableAttribute();
            }
        }
        
        public void Draw(PrimitiveType primitiveType)
        {
            foreach(ElementBufferObject ibo in iboList)
            {
                ibo.Draw(primitiveType);
            }
        }

        public void Draw(int first, int count, PrimitiveType primitiveType)
        {
            vbo.Draw(primitiveType, first, count);
        }

        public void Dispose()
        {
            if (vbo != null) vbo.Dispose();
            iboList.Clear();
            iboList = null;
            attributeList.Clear();
            attributeList = null;
            GL.DeleteVertexArray(handle);
            GC.SuppressFinalize(this);
        }

        ~VertexArrayObject()
        {
            if (vbo != null) vbo.Dispose();
            iboList.Clear();
            iboList = null;
            attributeList.Clear();
            attributeList = null;
            GL.DeleteVertexArray(handle);
        }

    }
}