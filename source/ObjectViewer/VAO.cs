using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace OpenBve
{
    /// <summary>
    /// Class to represent an openTK VertexArrayObject(VAO)
    /// </summary>
    internal class VertexArrayObject : IDisposable
    {
        private int handle;
        private Attribute[] attributelist;
        private VertexBufferObject vbo = null;
        private ElementBufferObject ibo = null;
        private List<ElementBufferObject> ibolist = null;
        private int attributecount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        internal VertexArrayObject()
        {
            GL.GenVertexArrays(1, out handle);
            attributelist = new Attribute[10];
        }
        /// <summary>
        /// Adds attributes to the VAO must be added in index order ie 0 to whatever
        /// </summary>
        /// <param name="index">Index of the attribute</param>
        /// <param name="attribute">The attribute object that is to be added</param>
        internal void AddAttribute(int index, Attribute attribute)
        {
#if DEBUG
            if(index < 0)
            {
                throw new IndexOutOfRangeException("cannot have an attribute with a negative index");
            }
            if(index != attributecount)
            {
                throw new IndexOutOfRangeException("Index not the next available one");
            }
#endif
            if(index > 9)
            {
                Array.Resize<Attribute>(ref attributelist, attributecount++);
            }
            attributelist[index] = attribute;
            attributecount++;
        }
        /// <summary>
        /// Adds a VBO object to the VAO needs to have one to draw, if a second is added it will replace the first and the first will be disposed of
        /// </summary>
        /// <param name="VBO">The VBO object to be added</param>
        internal void SetVBO(VertexBufferObject VBO)
        {
            if(vbo == null)
            {
                vbo = VBO;
            }
            else
            {
                vbo.UnBind();
                vbo.Dispose();
                vbo = VBO;
            }
            vbo.Bind();
        }
        /// <summary>
        /// Adds an EBO/IBO object to the VAO, it is not needed to Draw, if a second is added it will replace the first and the firsr will be disposed of 
        /// </summary>
        /// <param name="IBO">The EBO/IBO object to be added</param>
        internal void SetIBO(ElementBufferObject IBO)
        {
            if(ibo == null)
            {
                ibo = IBO;
            }
            else
            {
                ibo.UnBind();
                ibo.Dispose();
                ibo = IBO;
            }
            ibo.Bind();
        }
        /// <summary>
        /// Adds EBO/IBO list to the VAO
        /// </summary>
        /// <param name="IBO">The list of EBOs/IBOs</param>
        internal void SetIBO(List<ElementBufferObject>IBO)
        {
            ibolist = IBO;
        }
        /// <summary>
        /// Binds the VAO, activating it for use
        /// </summary>
        internal void Bind()
        {
            GL.BindVertexArray(handle);
        }
        /// <summary>
        /// Unbinds the VAO, deactivating it from use
        /// </summary>
        internal void UnBind()
        {
            GL.BindVertexArray(0);
        }
        /// <summary>
        /// Set the added attributes up must be called before drawing it
        /// </summary>
        internal void SetAttributes()
        {
            for (int n = 0; n < attributecount; n++)
            {
                attributelist[n].SetAttribute();
                attributelist[n].EnableAttribute();
            }
        }
        /// <summary>
        /// Draws the underlying EBO
        /// </summary>
        internal void Draw()
        {
            ibo.Draw();
        }
        /// <summary>
        /// Draws the underlying VBO
        /// </summary>
        /// <param name="first">The first vertex to start drawing from</param>
        /// <param name="count">The number of vertices to draw</param>
        internal void Draw(int first, int count)
        {
            vbo.Draw(first, count);
        }
        internal void Draw(ushort ibotodraw)
        {
            ibolist[ibotodraw].Draw();
        }
        /// <summary>
        /// Dispose method to clean up the VAO releasing the openGL VAO 
        /// </summary>
        public void Dispose()
        {
            if (ibo != null)
            {
                ibo.Dispose();
            }
            if (ibolist != null)
            {
                foreach(ElementBufferObject ibo in ibolist)
                {
                    ibo.Dispose();
                }
            }
            if (vbo != null)
            {
                vbo.Dispose();
            }
            attributelist = null;
            GL.DeleteVertexArray(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finaliser do not call used Dispose
        /// </summary>
        ~VertexArrayObject()
        {
            if (ibo != null)
            {
                ibo.Dispose();
            }
            if (ibolist != null)
            {
                foreach(ElementBufferObject ibo in ibolist)
                {
                    ibo.Dispose();
                }
            }
            if (vbo != null)
            {
                vbo.Dispose();
            }
            attributelist = null;
            GL.DeleteVertexArray(handle);
        }
    }
}