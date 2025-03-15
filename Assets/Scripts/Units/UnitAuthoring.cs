using Omniverse.Abilities;
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

				AddBuffer<WaypointBuffer>(entity);

				DynamicBuffer<Ability> abilities = AddBuffer<Ability>(entity);

				foreach (var abilityDesc in authoring.UnitDesc.Abilities)
				{
					Ability ability = new Ability()
					{
						Name = abilityDesc.Meta.Name,
						Icon = new WeakObjectReference<Sprite>(abilityDesc.Meta.Icon),
					};

					abilities.Add(ability);
				}
			}
		}
	}
}
