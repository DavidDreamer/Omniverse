using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public class BuildingUtils
	{
		public static int2 MapCellFromPosition(float3 position)
		{
			int x = (int)math.floor(position.x);
			int y = (int)math.floor(position.z);
			return new(x, y);
		}

		public static void Build(EntityCommandBuffer commandBuffer, BuildOperationData data)
		{
			Entity entity = commandBuffer.Instantiate(data.Building);
			commandBuffer.SetComponent(entity, data.LocalTransform);

			int2 position = MapCellFromPosition(data.LocalTransform.Position);

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
