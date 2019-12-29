using System.Collections.Generic;
using System;
using OpenBveApi.Objects;

namespace LibRender3
{
	internal class ObjectList :IDisposable
	{
		private List<StaticObject> staticObjectList;
		private ushort nextInstanceID;

		internal ObjectList()
		{
			staticObjectList = new List<StaticObject>();
			nextInstanceID = 0;
		}
		internal void AddObject(StaticObject obj)
		{
		
		}

		internal StaticObject[] GetUniqueStaticObjects()
		{
		}
	}
}
