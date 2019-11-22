using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;

namespace LibRender3
{
	internal class ElementBufferObject : IDisposable
	{
		private readonly int handle;
		private readonly ushort[] ibo;
		private readonly BufferUsageHint hint;

		internal ElementBufferObject(ushort[] ibo, BufferUsageHint hint)
		{
			GL.GenBuffers(1, out handle);
			this.ibo = ibo;
			this.hint = hint;
		}
		internal void BufferData()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
			GL.BufferData(BufferTarget.ElementArrayBuffer, ibo.Length * sizeof(ushort), ibo, hint);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
		internal void Bind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
		}
		internal void UnBind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
		public void Dispose()
		{
			GL.DeleteBuffer(handle);
			GC.SuppressFinalize(this);
		}
		~ElementBufferObject()
		{
			GL.DeleteBuffer(handle);
		}
		internal void Draw(PrimitiveType primitiveType)
		{
			GL.DrawElements(primitiveType, ibo.Length, DrawElementsType.UnsignedShort, ibo);
		}
		internal void Draw(PrimitiveType primitiveType, int instanceCount)
		{
			GL.DrawElementsInstanced(primitiveType, ibo.Length, DrawElementsType.UnsignedShort, ibo, instanceCount);
		}
		internal void Draw(PrimitiveType primitiveType, int instanceCount, int baseInstance)
		{
			GL.DrawElementsInstancedBaseInstance(primitiveType, ibo.Length, DrawElementsType.UnsignedShort, ibo, instanceCount, baseInstance);
		}
		internal void DrawIndirect(PrimitiveType primitiveType, IntPtr drawData)
		{
			GL.DrawElementsIndirect(primitiveType, DrawElementsType.UnsignedShort, drawData);
		}
	}
}
