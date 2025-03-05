using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Omniverse
{
	public class MapAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public MapSettings MapSettings { get; private set; }

		private class Baker : Baker<MapAuthoring>
		{
			public override void Bake(MapAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Map
				{
					Size = new int2(authoring.MapSettings.Size.x, authoring.MapSettings.Size.y)
				});
			}
		}
	}
}
