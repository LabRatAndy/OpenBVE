using System.Runtime.InteropServices;
using OpenTK;
using Matrix4D = OpenBveApi.Math.Matrix4D;
using OpenBveApi;

namespace LibRender3
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Uniform
	{
		internal Matrix4 modelTransform;
		internal Matrix4 viewTransform;
		internal Matrix4 projectionTransform;

		internal Uniform(Matrix4D model, Matrix4D view, Matrix4D projection)
		{
			modelTransform = ConvertToMatrix4(model);
			viewTransform = ConvertToMatrix4(view);
			projectionTransform = ConvertToMatrix4(projection);
		}
		internal static int SizeInBytes()
		{
			return Marshal.SizeOf(typeof(Uniform));
		}
		private static Matrix4 ConvertToMatrix4(Matrix4D input)
		{
			Vector4 row0 = new Vector4((float)input.Row0.X, (float)input.Row0.Y, (float)input.Row0.Z, (float)input.Row0.W);
			Vector4 row1 = new Vector4((float)input.Row1.X, (float)input.Row1.Y, (float)input.Row1.Z, (float)input.Row1.W);
			Vector4 row2 = new Vector4((float)input.Row2.X, (float)input.Row2.Y, (float)input.Row2.Z, (float)input.Row2.W);
			Vector4 row3 = new Vector4((float)input.Row3.X, (float)input.Row3.Y, (float)input.Row3.Z, (float)input.Row3.W);
			return new Matrix4(row0, row1, row2, row3);
		}
	}
}
