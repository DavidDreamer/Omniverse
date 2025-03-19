using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct NavAgentComponent : IComponentData
	{
		public Entity targetEntity;
		public float3 targetPosition;
		public int currentWaypoint;
		public bool IsActive;
	}
}
