using Unity.Entities;
using Unity.Entities.Content;
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
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddSharedComponent(entity, new Faction()
				{
					ID = authoring.FactionID
				});

				AddComponent(entity, new Health()
				{
					Maximum = 100,
					Current = 100
				});

				AddComponent(entity, new MovementSpeed()
				{
					Current = 5
				});

				AddComponent(entity, new NavAgentComponent());
				AddComponentObject(entity, new CommandModule());

				AddBuffer<WaypointBuffer>(entity);

				var abilityModule = new AbilityModule();

				foreach (var desc in authoring.UnitDesc.Abilities)
				{
					Ability ability = new()
					{
						Name = desc.Meta.Name,
						Icon = new WeakObjectReference<Sprite>(desc.Meta.Icon),
						Cooldown = new Cooldown
						{
							Time = desc.Cooldown.Time
						},
						Target = desc.Target,
						CastRange = desc.Casting.Range
					};

					abilityModule.Abilities.Add(ability);
				}

				AddComponentObject(entity, abilityModule);
			}
		}
	}
}
