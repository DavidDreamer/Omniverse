using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse
{
	public struct NavAgentComponent : IInputComponentData
	{
		public Entity targetEntity;
		public float3 targetPosition;
		public int currentWaypoint;
		public bool IsActive;
	}
}
