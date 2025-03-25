using Unity.Entities;

namespace Omniverse
{
	public readonly partial struct Unit : IAspect
	{
		public readonly DynamicEntity DynamicEntity;

		public readonly RefRW<Faction> Faction;
	}
}
