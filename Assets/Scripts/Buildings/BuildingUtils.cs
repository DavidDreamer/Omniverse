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
				Name = data.Desc.Value.Meta.Name,
				Icon = data.Desc.Value.Meta.Icon
			});
			commandBuffer.AddComponent<Building>(entity);
		}
	}
}
