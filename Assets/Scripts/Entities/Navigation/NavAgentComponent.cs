using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct NavAgentComponent : IComponentData
	{
		public Entity targetEntity;
		public float3 targetPosition;
		public bool pathCalculated;
		public int currentWaypoint;
		public float moveSpeed;
		public float nextPathCalculateTime;
	}
}
