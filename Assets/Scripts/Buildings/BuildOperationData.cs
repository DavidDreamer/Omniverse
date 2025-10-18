using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	public struct BuildOperationData
	{
		public Entity Building;
		public LocalTransform LocalTransform;
		public Faction Faction;
	}
}
