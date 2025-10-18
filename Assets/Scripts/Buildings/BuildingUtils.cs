using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public class BuildingUtils
	{
		public static void Build(EntityCommandBuffer commandBuffer, BuildOperationData data)
		{
			Entity entity = commandBuffer.Instantiate(data.Building);
			commandBuffer.SetComponent(entity, data.LocalTransform);

			int2 position = new((int)data.LocalTransform.Position.x, (int)data.LocalTransform.Position.z);
			commandBuffer.AddComponent(entity, new Obstacle
			{
				Start = position,
				End = position
			});

			commandBuffer.SetComponent<Faction>(entity, new()
			{
				ID = data.Faction
			});
		}
	}
}
