using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class BuildingAuthoring : MonoBehaviour
	{
		public BuildingDesc Desc;

		private class Baker : Baker<BuildingAuthoring>
		{
			public override void Bake(BuildingAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Building>(entity);
			}
		}
	}
}
