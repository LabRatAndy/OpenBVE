using System.Runtime.InteropServices;
using OpenTK;

namespace LibRender3
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Uniform
	{
		internal Matrix4 modelTransform;
		internal Matrix4 viewTransform;
		internal Matrix4 projectionTransform;

		internal Uniform(Matrix4d model, Matrix4d view, Matrix4d projection)
		{
		}
		internal static int SizeInBytes()
		{
			return Marshal.SizeOf(typeof(Uniform));
		}
	}
}
