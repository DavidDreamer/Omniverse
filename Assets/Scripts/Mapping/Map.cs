using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using VContainer;
using VContainer.Unity;

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

		private ComputeBuffer PropertiesBuffer { get; set; }

		private MapShaderProperties[] PropertiesData { get; set; }

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

			PropertiesBuffer = new ComputeBuffer(1, UnsafeUtility.SizeOf<MapShaderProperties>(), ComputeBufferType.Constant);
			Shader.SetGlobalConstantBuffer(ShaderVariables.MapProperties, PropertiesBuffer, 0, PropertiesBuffer.stride);

			PropertiesData = new MapShaderProperties[1];
			PropertiesData[0] = mapShaderProperties;
			PropertiesBuffer.SetData(PropertiesData);
		}

		public void Dispose()
		{
			if (Camera != null)
			{
				Camera.targetTexture = null;
			}

			Destroy(RenderTexture);

			PropertiesBuffer.Release();
		}
	}
}
