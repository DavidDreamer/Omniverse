using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Omniverse
{
	public class SpectatorsAuthoring : MonoBehaviour
	{
		public GameObject Prefab;

		public Transform[] SpawnPoints;

		private class Baker : Baker<SpectatorsAuthoring>
		{
			public override void Bake(SpectatorsAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);

				var prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);

				AddComponent(entity, new Spectators
				{
					Prefab = prefab
				});

				AddBuffer<SpawnPoint>(entity);

				foreach (Transform point in authoring.SpawnPoints)
				{
					SpawnPoint spawnPoint = new()
					{
						Position = (float3)point.position,
						Rotation = (quaternion)point.rotation
					};

					AppendToBuffer(entity, spawnPoint);
				}
			}
		}
	}
}
