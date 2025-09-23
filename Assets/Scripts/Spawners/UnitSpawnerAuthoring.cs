using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Omniverse
{
	public class UnitSpawner : IComponentData
	{
		public Entity Unit;

		public float3[] SpawnPoints;
	}

	public class UnitSpawnerAuthoring : MonoBehaviour
	{
		public GameObject Prefab;

		public Transform[] SpawnPoints;

		private class Baker : Baker<UnitSpawnerAuthoring>
		{
			public override void Bake(UnitSpawnerAuthoring authoring)
			{
				var unitSpawner = new UnitSpawner
				{
					Unit = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
					SpawnPoints = authoring.SpawnPoints.Select(point => (float3)point.position).ToArray()
				};

				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponentObject(entity, unitSpawner);
			}
		}
	}
}
