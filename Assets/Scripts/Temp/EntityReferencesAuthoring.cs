using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public struct EntityReferences : IComponentData
	{
		public Entity Fireball;
	}

	public class EntityReferencesAuthoring : MonoBehaviour
	{
		public GameObject Fireball;

		private class Baker : Baker<EntityReferencesAuthoring>
		{
			public override void Bake(EntityReferencesAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);
				var fireball = GetEntity(authoring.Fireball, TransformUsageFlags.Dynamic);
				AddComponent(entity, new EntityReferences
				{
					Fireball = fireball
				});
			}
		}
	}
}
