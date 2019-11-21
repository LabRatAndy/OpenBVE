using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LibRender3
{
	internal class VertexBufferObject : IDisposable
	{
		private readonly int handle;
		private readonly BufferUsageHint hint;
		private readonly RenderVertex[] vertexData;

		internal VertexBufferObject(RenderVertex[] vertexData, BufferUsageHint hint)
		{
			GL.GenBuffers(1, out handle);
			this.vertexData = vertexData;
			this.hint = hint;
		}
		internal void BufferData()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
			GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * RenderVertex.SizeInBytes, vertexData, hint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
		internal void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
		}
		internal void UnBind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
		public void Dispose()
		{
			GL.DeleteBuffer(handle);
			GC.SuppressFinalize(this);
		}
		~VertexBufferObject()
		{
			GL.DeleteBuffer(handle);
		}
		internal void Draw(PrimitiveType primitiveType)
		{
			GL.DrawArrays(primitiveType, 0, vertexData.Length);
		}
		internal void Draw(PrimitiveType primitiveType, int instanceCount)
		{
			GL.DrawArraysInstanced(primitiveType, 0, vertexData.Length, instanceCount);
		}
		internal void Draw(PrimitiveType primitiveType, int instanceCount, int baseInstance)
		{
			GL.DrawArraysInstancedBaseInstance(primitiveType, 0, vertexData.Length, instanceCount, baseInstance);
		}
		internal void DrawIndirect(PrimitiveType primitiveType, IntPtr drawData)
		{
			GL.DrawArraysIndirect(primitiveType, drawData);
		}
	}
}
