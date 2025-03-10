using UnityEngine;
using Unity.Entities;

namespace Omniverse
{
	public class SpawnerAuthoring : MonoBehaviour
	{
		public GameObject Prefab;
		public float Interval;
		public float Speed;

		private class Baker : Baker<SpawnerAuthoring>
		{
			public override void Bake(SpawnerAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				Entity missile = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);

				AddComponent(entity, new Spawner
				{
					Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
					Interval = authoring.Interval,
					Speed = authoring.Speed,
					Position = authoring.transform.position
				});
			}
		}
	}
}
