using System.Runtime.InteropServices;
using OpenBveApi.Math;

namespace LibRender3
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct RenderVertex
	{
		internal Vector3f Coordinates;
		internal Vector3f Normals;
		internal Vector2f TexCoordinates;

		internal static int SizeInBytes => Marshal.SizeOf(typeof(RenderVertex));
	}

}
