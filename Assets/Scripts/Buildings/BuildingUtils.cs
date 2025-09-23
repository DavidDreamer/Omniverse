using Unity.Entities;

namespace Omniverse
{
	public class BuildingUtils
	{
		public static void Build(EntityCommandBuffer commandBuffer, BuildOperationData data)
		{
			Entity entity = commandBuffer.Instantiate(data.Entity);
			commandBuffer.SetComponent(entity, data.LocalTransform);
			commandBuffer.AddComponent(entity, data.Faction);

			commandBuffer.AddComponent(entity, new MetaData
			{
				Icon = data.Desc.Value.Meta.Icon,
				Name = data.Desc.Value.Meta.Name,
			});

			commandBuffer.AddComponent(entity, new Building
			{
				Desc = data.Desc
			});
		}
	}
}
