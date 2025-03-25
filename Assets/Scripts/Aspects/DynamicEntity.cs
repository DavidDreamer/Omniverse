using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	public readonly partial struct DynamicEntity : IAspect
	{
		public readonly Entity Entity;

		public readonly RefRW<LocalTransform> LocalTransform;

		public readonly RefRW<LocalToWorld> LocalToWorld;
	}
}
