using Omniverse.Mapping;
using Omniverse.Rendering;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class MiniMapWidget : Widget
	{
		public RawImage Image;
		public RawImage FogOfWar;

		public override void Initialize(EntityManager entityManager)
		{
			base.Initialize(entityManager);

			var minimapRenderSystem = EntityManager.World.GetExistingSystemManaged<MinimapRenderSystem>();
			Image.texture = minimapRenderSystem.RenderTexture;

			var fogOfWarRenderer = FindFirstObjectByType<FogOfWarRenderer>(FindObjectsInactive.Include);
			FogOfWar.texture = fogOfWarRenderer.BlurRT2;
		}
	}
}