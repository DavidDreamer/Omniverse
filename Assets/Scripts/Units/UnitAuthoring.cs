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
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new Faction()
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
			}
		}
	}
}
