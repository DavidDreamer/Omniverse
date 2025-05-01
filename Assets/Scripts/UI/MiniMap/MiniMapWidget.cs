using Omniverse.Mapping;
using Omniverse.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class MiniMapWidget : Widget
	{
		public RawImage Image;
		public RawImage FogOfWar;

		public MiniMapCameraBounds CameraBounds;

		public void Start()
		{
			var mapRenderer = FindFirstObjectByType<MapRenderer>(FindObjectsInactive.Include);
			Image.texture = mapRenderer.RenderTexture;

			var fogOfWarRenderer = FindFirstObjectByType<FogOfWarRenderer>(FindObjectsInactive.Include);
			FogOfWar.texture = fogOfWarRenderer.BlurRT2;
		}
	}
}