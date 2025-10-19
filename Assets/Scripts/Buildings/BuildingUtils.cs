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

			int x = (int)math.floor(data.LocalTransform.Position.x);
			int y = (int)math.floor(data.LocalTransform.Position.z);
			int2 position = new(x, y);

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
