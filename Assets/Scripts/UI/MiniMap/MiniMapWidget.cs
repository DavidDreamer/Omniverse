using Omniverse.Rendering.FogOfWar;
using Omniverse.Mapping;
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
		private Map Map { get; set; }

		[Inject]
		private FogOfWarRenderer FogOfWarRenderer { get; set; }

		public void Initialize()
		{
			Image.texture = Map.RenderTexture;
			FogOfWar.texture = FogOfWarRenderer.BlurRT2;
		}
	}
}