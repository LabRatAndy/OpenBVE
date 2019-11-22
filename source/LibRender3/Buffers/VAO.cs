using System;
using OpenTK.Graphics.OpenGL;

namespace LibRender3
{
	internal class VertexArrayObject : IDisposable
	{
		private readonly int handle;
		private VertexBufferObject vbo = null;
		private ElementBufferObject ibo = null;

		internal VertexArrayObject()
		{
			GL.GenVertexArrays(1, out handle);
		}

		internal void Bind()
		{
			GL.BindVertexArray(handle);
		}
		internal void UnBind()
		{
			GL.BindVertexArray(0);
		}
		internal void SetVBO(VertexBufferObject VBO)
		{
			if (vbo != null)
			{
				UnBind();
				vbo.Dispose();
				Bind();
			}
			vbo = VBO;
			vbo.Bind();
			vbo.BufferData();
		}
		internal void SetIBO(ElementBufferObject IBO)
		{
			if (ibo != null)
			{
				UnBind();
				ibo.Dispose();
				Bind();
			}
			ibo = IBO;
			ibo.Bind();
			ibo.BufferData();
		}
		public void Dispose()
		{
			if (ibo != null) ibo.Dispose();
			if (vbo != null) vbo.Dispose();
			GL.DeleteVertexArray(handle);
			GC.SuppressFinalize(this);
		}
		~VertexArrayObject()
		{
			if (ibo != null) ibo.Dispose();
			if (vbo != null) vbo.Dispose();
			GL.DeleteVertexArray(handle);
		}
		internal void DrawVBO(PrimitiveType primitiveType)
		{
			vbo.Draw(primitiveType);
		}
		internal void DrawVBO(PrimitiveType primitiveType, int instanceCount)
		{
			vbo.Draw(primitiveType, instanceCount);
		}
		internal void DrawVBO(PrimitiveType primitiveType, int instanceCount, int baseInstance)
		{
			vbo.Draw(primitiveType, instanceCount, baseInstance);
		}
		internal void DrawVBOIndirect(PrimitiveType primitiveType, IntPtr drawData)
		{
			vbo.DrawIndirect(primitiveType, drawData);
		}
		internal void DrawIBO(PrimitiveType primitiveType)
		{
			ibo.Draw(primitiveType);
		}
		internal void DrawIBO(PrimitiveType primitiveType, int instanceCount)
		{
			ibo.Draw(primitiveType, instanceCount);
		}
		internal void DrawIBO(PrimitiveType primitiveType, int instanceCount, int baseInstance)
		{
			ibo.Draw(primitiveType, instanceCount, baseInstance);
		}
		internal void DrawIBOIndirect(PrimitiveType primitiveType, IntPtr drawData)
		{
			ibo.DrawIndirect(primitiveType, drawData);
		}
	}
}
