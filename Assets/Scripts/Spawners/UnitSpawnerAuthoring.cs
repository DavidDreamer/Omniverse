using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public struct UnitSpawner : IComponentData
	{
		public Entity Unit;

		public Entity Ability;

		public Entity Ability2;
	}

	public class UnitSpawnerAuthoring : MonoBehaviour
	{
		public GameObject Prefab;

		public GameObject Ability;

		public GameObject Ability2;

		private class Baker : Baker<UnitSpawnerAuthoring>
		{
			public override void Bake(UnitSpawnerAuthoring authoring)
			{
				var unitSpawner = new UnitSpawner
				{
					Unit = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
					Ability = GetEntity(authoring.Ability, TransformUsageFlags.None),
					Ability2 = GetEntity(authoring.Ability2, TransformUsageFlags.None)
				};

				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, unitSpawner);

			}
		}
	}
}
