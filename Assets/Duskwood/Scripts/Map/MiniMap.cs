using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class MiniMap: MonoBehaviour, IInitializable, IDisposable
	{
		[field: SerializeField]
		private UnityEngine.Camera Camera { get; set; }

		[Inject]
		private MapSettings MapSettings { get; set; }

		public RenderTexture RenderTexture { get; private set; }

		public void Initialize()
		{
			RenderTexture = new RenderTexture(
				MapSettings.Size.x,
				MapSettings.Size.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			Camera.targetTexture = RenderTexture;

			Camera.transform.position = new Vector3(MapSettings.Size.x / 2f, Camera.transform.position.y,
				MapSettings.Size.y / 2f);
		}

		public void Dispose()
		{
			if (Camera != null)
			{
				Camera.targetTexture = null;
			}

			Destroy(RenderTexture);
		}
	}
}
