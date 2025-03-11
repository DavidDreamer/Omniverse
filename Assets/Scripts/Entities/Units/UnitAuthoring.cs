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
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Faction());
			}
		}
	}
}
