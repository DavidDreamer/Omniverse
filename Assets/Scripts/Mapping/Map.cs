using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using VContainer;
using VContainer.Unity;
using Dreambox.Rendering.Core;

namespace Omniverse.Mapping
{
	public class Map : MonoBehaviour, IInitializable, IDisposable
	{
		private static class ShaderVariables
		{
			public static int MapProperties { get; } = Shader.PropertyToID(nameof(MapProperties));
		}

		[field: SerializeField]
		private UnityEngine.Camera Camera { get; set; }

		[Inject]
		public MapSettings MapSettings { get; set; }

		public RenderTexture RenderTexture { get; private set; }

		private ConstantComputeBuffer<MapShaderProperties> PropertiesBuffer { get; set; }

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

			Camera.orthographicSize = MapSettings.Size.x / 2f;

			Camera.transform.position = new Vector3(MapSettings.Size.x / 2f, Camera.transform.position.y,
				MapSettings.Size.y / 2f);

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(MapSettings.Size.x, MapSettings.Size.y, 0, 0)
			};

			PropertiesBuffer = new ConstantComputeBuffer<MapShaderProperties>(ShaderVariables.MapProperties);
			PropertiesBuffer.SetData(mapShaderProperties);
		}

		public void Dispose()
		{
			if (Camera != null)
			{
				Camera.targetTexture = null;
			}

			Destroy(RenderTexture);

			PropertiesBuffer.Dispose();
		}
	}
}
