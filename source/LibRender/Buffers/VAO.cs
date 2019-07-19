using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenBveApi.LibRender;

namespace LibRender
{
    /// <summary>
    /// Class to repressenting an openGL VertexArrayObject
    /// </summary>
    public class VertexArrayObject : IDisposable
    {
        /// <summary>
        /// openGL handle to the VAO object
        /// </summary>
        private int handle;
        /// <summary>
        /// List of attributes attached to the VAO
        /// </summary>
        private List<Attribute> attributeList = null;
        /// <summary>
        /// VBO attached to the VAO
        /// </summary>
        private VertexBufferObject vbo = null;
        /// <summary>
        /// List of EBO attached to the VAO
        /// </summary>
        private List<ElementBufferObject> iboList = null;

        /// <summary>
        /// Constructs the Vertex Array Object
        /// </summary>
        /// <param name="Attributes">List of attributes to attached the VAO object</param>
        /// <param name="ibos">List of EBO attached to the VAO object</param>
        /// <param name="VBO">VBO object attached to the VAO object</param>
        public VertexArrayObject(List<Attribute>Attributes, List<ElementBufferObject> ibos, VertexBufferObject VBO)
        {
            GL.GenVertexArrays(1, out handle);
            attributeList = Attributes;
            iboList = ibos;
            vbo = VBO;
            vbo.Bind();
            foreach (ElementBufferObject ibo in iboList)
            {
                ibo.Bind();
            }
        }
        public VertexArrayObject(List<Attribute>attributes)
        {
            GL.GenVertexArrays(1, out handle);
            attributeList = attributes;
        }

        /// <summary>
        /// Binds / activates the VAO for use
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Sets the attached attributes in openGL
        /// </summary>
        public void SetAttributes()
        {
            foreach(Attribute attrib in attributeList)
            {
                attrib.SetAttribute();
                attrib.EnableAttribute();
            }
        }
        
        /// <summary>
        /// Draws the the attached EBOs
        /// </summary>
        /// <param name="primitiveType">The primitive type that forms the EBO</param>
        public void Draw(PrimitiveType primitiveType)
        {
            foreach(ElementBufferObject ibo in iboList)
            {
                ibo.Draw(primitiveType);
            }
        }

        /// <summary>
        /// Draws the attached VBO
        /// </summary>
        /// <param name="first">The first vertex in the VBO to start  drawing from </param>
        /// <param name="count">The number of vertices to draw</param>
        /// <param name="primitiveType">The primitive type that forms the object in the VBO</param>
        public void Draw(int first, int count, PrimitiveType primitiveType)
        {
            vbo.Draw(primitiveType, first, count);
        }

        /// <summary>
        /// Cleans up the VAO handle and the attached VBO, EBO list and attribute list 
        /// </summary>
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

        /// <summary>
        /// Finaliser
        /// </summary>
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