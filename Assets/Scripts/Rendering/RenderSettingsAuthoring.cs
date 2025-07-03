using Dreambox.Rendering;
using Omniverse.Mapping;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class RenderSettingsAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public SelectionRenderSettings Selection { get; set; }

		[field: SerializeField]
		public SelectionBoxRenderSettings SelectionBox { get; set; }

		[field: SerializeField]
		public MinimapRenderSettings Minimap { get; set; }

		[field: SerializeField]
		public CursorRenderSettings Cursor { get; set; }

		[field: SerializeField]
		public FogOfWarRenderSettings FogOfWar { get; set; }

		[field: SerializeField]
		public AbilityRenderSettings Ability { get; set; }

		[field: SerializeField]
		public HealthBarRenderSettings HealthBar { get; set; }

		[field: SerializeField]
		public NavigationRenderSettings Navigation { get; set; }

		[field: SerializeField]
		public OutlineRenderSettings Outline { get; set; }

		[field: SerializeField]
		public BuilderRenderSettings Builder { get; set; }

		private class Baker : Baker<RenderSettingsAuthoring>
		{
			public override void Bake(RenderSettingsAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new RenderSettings
				{
					Selection = authoring.Selection,
					SelectionBox = authoring.SelectionBox,
					Minimap = authoring.Minimap,
					Cursor = authoring.Cursor,
					FogOfWar = authoring.FogOfWar,
					Ability = authoring.Ability,
					HealthBar = authoring.HealthBar,
					Navigation = authoring.Navigation,
					Outline = authoring.Outline,
					Builder = authoring.Builder,
				});
			}
		}
	}
}
