using OpenBveApi.Objects;
using OpenBveApi.Math;

namespace LibRender3
{
	internal struct ObjectInstance
	{
		internal StructureType type;
		internal ushort uniqueObjectIndex;
		internal ushort uniqueObjectInstanceID;
		internal Matrix4D modelTransform;
		internal bool visible;

		internal ObjectInstance(StructureType type, ushort objectindex, ushort objectinstance, Matrix4D transform, bool visible)
		{
			this.type = type;
			uniqueObjectIndex = objectindex;
			uniqueObjectInstanceID = objectinstance;
			modelTransform = transform;
			this.visible = visible;
		}
	}
}
