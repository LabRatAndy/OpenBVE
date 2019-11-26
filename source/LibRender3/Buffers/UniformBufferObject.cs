using System;
using OpenTK.Graphics.OpenGL;

namespace LibRender3
{
	internal class UniformBufferObject : IDisposable
	{
		private Uniform[] uniforms;
		private readonly int handle;
		private readonly int lengthofarray;

		internal UniformBufferObject(Uniform[] UBOs)
		{
			GL.GenBuffers(1, out handle);
			uniforms = UBOs;
			lengthofarray = UBOs.Length;
		}
		internal void SetBufferStrorage()
		{
			Bind();
			GL.BufferStorage(BufferTarget.UniformBuffer, uniforms.Length * Uniform.SizeInBytes(), uniforms, BufferStorageFlags.MapWriteBit| 
				BufferStorageFlags.MapCoherentBit| BufferStorageFlags.MapPersistentBit );
			UnBind();
		}
		internal void Bind()
		{
			GL.BindBuffer(BufferTarget.UniformBuffer, handle);
		}
		internal void UnBind()
		{
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		}
		internal Uniform[] Uniforms
		{
			get
			{
				return uniforms;
			}
			set
			{
				if(value.Length > lengthofarray) throw new OverflowException("Uniforms array cannot be set longer than the first uniform array");
				uniforms = value;
			}
		}
		public void Dispose()
		{
			GL.DeleteBuffer(handle);
			GC.SuppressFinalize(this);
		}
		~UniformBufferObject()
		{
			GL.DeleteBuffer(handle);
		}

	}
}
