using Omniverse.Mapping;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class RenderSettingsAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public SelectionBoxRenderSettings SelectionBox { get; set; }

		[field: SerializeField]
		public MinimapRenderSettings Minimap { get; set; }

		[field: SerializeField]
		public CursorRenderSettings Cursor { get; set; }

		private class Baker : Baker<RenderSettingsAuthoring>
		{
			public override void Bake(RenderSettingsAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new RenderSettings
				{
					SelectionBox = authoring.SelectionBox,
					Minimap = authoring.Minimap,
					Cursor = authoring.Cursor
				});
			}
		}
	}
}
