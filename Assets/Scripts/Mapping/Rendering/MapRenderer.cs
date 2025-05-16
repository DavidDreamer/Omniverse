using Dreambox.Rendering.Core;
using Omniverse.Rendering;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Omniverse.Mapping
{
	public class MapRenderer : RenderFeature
	{
		private static class ShaderVariables
		{
			public static int MapProperties { get; } = Shader.PropertyToID(nameof(MapProperties));
		}

		[field: SerializeField]
		private Camera Camera { get; set; }

		public RenderTexture RenderTexture { get; private set; }

		private ConstantComputeBuffer<MapShaderProperties> PropertiesBuffer { get; set; }

		public void OnEnable()
		{
			var gameOptions = EntityManager.GetSingletonManaged<GameOptions>();

			RenderTexture = new RenderTexture(
				gameOptions.MapSize.x,
				gameOptions.MapSize.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			Camera.targetTexture = RenderTexture;

			Camera.orthographicSize = gameOptions.MapSize.x / 2f;

			Camera.transform.position = new Vector3(0, Camera.transform.position.y, 0);

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(gameOptions.MapSize.x, gameOptions.MapSize.y, 0, 0)
			};

			PropertiesBuffer = new ConstantComputeBuffer<MapShaderProperties>(ShaderVariables.MapProperties);
			PropertiesBuffer.SetData(mapShaderProperties);
		}

		public void OnDisable()
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
