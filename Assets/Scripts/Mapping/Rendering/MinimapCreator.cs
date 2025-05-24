using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Mapping
{
	public class MinimapCreator : EditorWindow
	{
		[MenuItem("Tools/Minimap Creator")]
		public static void CreateWindow()
		{
			var window = GetWindow<MinimapCreator>();
			window.Show();
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Create"))
			{
				Create();
			}
		}

		private void Create()
		{
			Camera camera = default;
			RenderTexture renderTexture = default;

			try
			{
				int mapWidth = 64;
				int mapHeight = 64;

				int width = 512;
				int height = 512;

				camera = new GameObject().AddComponent<Camera>();
				camera.orthographic = true;
				camera.orthographicSize = math.max(mapWidth, mapHeight) / 2;

				camera.transform.position = new Vector3(0, 10, 0);
				camera.transform.eulerAngles = new Vector3(90, 0, 0);
		
				renderTexture = new RenderTexture(
				width,
				height,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
				{
					name = "Minimap"
				};

				camera.targetTexture = renderTexture;
				
				camera.Render();

				Texture2D texture = new(width, height, TextureFormat.RGBA32, false);
				RenderTexture.active = renderTexture;
				var rect = new Rect(0, 0, width, height);

				texture.ReadPixels(rect, 0, 0);
				texture.Apply();

				byte[] data = texture.EncodeToPNG();

				File.WriteAllBytes("Assets/t.png", data);
				AssetDatabase.Refresh();

				Debug.Log("Done");
			}
			finally
			{
				RenderTexture.active = null;

				if (camera != null)
				{
					DestroyImmediate(camera.gameObject);
				}

				if (renderTexture != null)
				{
					renderTexture.Release();
				}
			}
		}
	}
}
