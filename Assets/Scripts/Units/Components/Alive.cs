using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public struct Alive : IComponentData, IEnableableComponent
	{
	}
}
