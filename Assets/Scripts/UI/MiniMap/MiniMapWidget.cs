using Omniverse.Mapping;
using Omniverse.Rendering;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class MiniMapWidget : MonoBehaviour, IInitializable
	{
		public RawImage Image;
		public RawImage FogOfWar;

		public MiniMapCameraBounds CameraBounds;

		[Inject]
		private MapRenderer MapRenderer { get; set; }

		[Inject]
		private FogOfWarRenderer FogOfWarRenderer { get; set; }

		public void Initialize()
		{
			Image.texture = MapRenderer.RenderTexture;
			FogOfWar.texture = FogOfWarRenderer.BlurRT2;
		}
	}
}