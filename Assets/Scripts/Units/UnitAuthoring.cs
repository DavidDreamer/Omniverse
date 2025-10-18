using Omniverse.Abilities;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class UnitAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public UnitDesc UnitDesc { get; set; }

		private class Baker : Baker<UnitAuthoring>
		{
			public override void Bake(UnitAuthoring authoring)
			{
				UnitDesc desc = authoring.UnitDesc;

				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new MetaData
				{
					Icon = desc.Icon,
					Name = desc.Name,
				});

				AddComponent<Faction>(entity);

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

				AddComponent(entity, new Movement()
				{
					Speed = new()
					{
						Base = authoring.UnitDesc.Movement.Speed
					}
				});

				AddBuffer<Effect>(entity);
				AddBuffer<Ability>(entity);
				AddComponent<AbilityInput>(entity);

				foreach (var abilityDesc in desc.Abilities)
				{
					var ability = new Ability()
					{
						Desc = abilityDesc,
						Manacost = new()
						{
							Value = abilityDesc.Manacost.Value,
							Mode = abilityDesc.Manacost.Mode
						},
						Cooldown = new()
						{
							Duration = abilityDesc.Cooldown.Duration
						},
						Casting = new()
						{
							Range = abilityDesc.Casting.Range,
							Time = abilityDesc.Casting.Time
						}
					};

					AppendToBuffer(entity, ability);
				}

				if (desc.BuildAbilityDesc != null)
				{
					AddBuffer<Blueprint>(entity);

					foreach (var prefab in desc.BuildAbilityDesc.Buildings)
					{
						Entity building = GetEntity(prefab, TransformUsageFlags.Dynamic);

						Blueprint blueprint = new()
						{
							Building = building,
						};

						AppendToBuffer(entity, blueprint);
					}
				}

				AddComponent<Unit>(entity);
				AddComponent<UnitInput>(entity);
			}
		}
	}
}
