using Omniverse.Mapping;
using Unity.Entities;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class MiniMapWidget : Widget
	{
		public RawImage Image;

		public override void Initialize(EntityManager entityManager)
		{
			base.Initialize(entityManager);

			var minimapRenderSystem = EntityManager.World.GetExistingSystemManaged<MinimapRenderSystem>();
			Image.texture = minimapRenderSystem.RenderTexture;
		}
	}
}