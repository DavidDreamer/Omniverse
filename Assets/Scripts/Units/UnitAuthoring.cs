using Omniverse.Abilities;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class UnitAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public UnitDesc UnitDesc { get; set; }

		public int FactionID;

		private class Baker : Baker<UnitAuthoring>
		{
			public override void Bake(UnitAuthoring authoring)
			{
				UnitDesc desc = authoring.UnitDesc;

				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new Faction()
				{
					ID = authoring.FactionID
				});

				AddComponent(entity, new Alive());

				AddComponent(entity, new Health()
				{
					Maximum = desc.Health.Amount,
					Current = desc.Health.Amount
				});

				if (desc.Invulnerable)
				{
					AddComponent<Invulnerable>(entity);
				}

				AddComponent(entity, new Mana()
				{
					Maximum = desc.Mana.Amount,
					Current = desc.Mana.Amount
				});

				AddComponent(entity, new MovementSpeed()
				{
					Base = authoring.UnitDesc.Movement.BaseSpeed,
					Current = authoring.UnitDesc.Movement.BaseSpeed
				});

				AddBuffer<Effect>(entity);

				AddComponent(entity, new NavAgentComponent());
				AddComponentObject(entity, new CommandModule());
				AddBuffer<AbilityReference>(entity);

				if (desc.BuildAbilityDesc != null)
				{
					var building = GetEntity(desc.BuildAbilityDesc.Prefab, TransformUsageFlags.Dynamic);
					AddComponent(entity, new BuildAbility()
					{
						Desc = desc.BuildAbilityDesc,
						Building = building
					});
				}
			}
		}
	}
}
