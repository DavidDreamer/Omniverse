using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class BuildingAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public BuildingDesc Desc { get; set; }

		private class Baker : Baker<BuildingAuthoring>
		{
			public override void Bake(BuildingAuthoring authoring)
			{
				BuildingDesc desc = authoring.Desc;

				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent<Building>(entity);

				AddComponent(entity, new MetaData
				{
					Icon = desc.Meta.Icon,
					Name = desc.Meta.Name,
				});

				AddComponent<Faction>(entity);
			}
		}
	}
}
