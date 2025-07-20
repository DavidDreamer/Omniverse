using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	public struct BuildOperationData
	{
		public UnityObjectRef<BuildingDesc> Desc;
		public Entity Entity;
		public LocalTransform LocalTransform;
		public Faction Faction;
	}
}
