using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public struct UnitSpawner : IComponentData
	{
		public Entity Unit;
	}

	public class UnitSpawnerAuthoring : MonoBehaviour
	{
		public GameObject Prefab;

		private class Baker : Baker<UnitSpawnerAuthoring>
		{
			public override void Bake(UnitSpawnerAuthoring authoring)
			{
				var unitSpawner = new UnitSpawner
				{
					Unit = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
				};

				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, unitSpawner);
			}
		}
	}
}
