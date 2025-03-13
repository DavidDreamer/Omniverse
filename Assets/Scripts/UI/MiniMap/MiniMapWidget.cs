using Omniverse.Mapping;
using Omniverse.Rendering;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class MiniMapWidget : MonoBehaviour, IInitializable
	{
		public RawImage Image;
		public RawImage FogOfWar;

		public MiniMapCameraBounds CameraBounds;

		public void Initialize()
		{
			var mapRenderer = FindFirstObjectByType<MapRenderer>(FindObjectsInactive.Include);
			Image.texture = mapRenderer.RenderTexture;

			var fogOfWarRenderer = FindFirstObjectByType<FogOfWarRenderer>(FindObjectsInactive.Include);
			FogOfWar.texture = fogOfWarRenderer.BlurRT2;
		}
	}
}