using Omniverse.Abilities;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Omniverse
{
	public class AbilityAuthoring : MonoBehaviour
	{
		public AbilityDesc Desc;

		private class Baker : Baker<AbilityAuthoring>
		{
			public override void Bake(AbilityAuthoring authoring)
			{
				AbilityDesc desc = authoring.Desc;

				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Ability());

				AddComponent(entity, new MetaData
				{
					Name = desc.Meta.Name,
					Icon = new WeakObjectReference<Sprite>(desc.Meta.Icon),
				});

				AddComponent(entity, new Cooldown
				{
					Duration = desc.Cooldown.Time
				});

				AddComponent(entity, new Casting()
				{
					Time = desc.Casting.Time
				});

				AddComponentObject(entity, new AbilityTarget()
				{
					Target = desc.Target
				});

				AddComponent(entity, new CastRange()
				{
					Value = desc.Casting.Range
				});

				AddComponentObject(entity, new AbilityActiveOperation
				{
					Operation = desc.ActiveOperation
				});

				AddComponent<AbilityInput>(entity);
				AddComponent<Owner>(entity);
			}
		}
	}
}
