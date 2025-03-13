using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Omniverse.Mapping
{
	public class MapRenderer : MonoBehaviour
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
			var map = ECSUtils.GetSingleton<Map>();

			RenderTexture = new RenderTexture(
				map.Size.x,
				map.Size.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			Camera.targetTexture = RenderTexture;

			Camera.orthographicSize = map.Size.x / 2f;

			Camera.transform.position = new Vector3(map.Size.x / 2f, Camera.transform.position.y, map.Size.y / 2f);

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(map.Size.x, map.Size.y, 0, 0)
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
