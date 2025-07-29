using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

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
				var gameOptions = FindAnyObjectByType<GameOptionsAuthoring>(FindObjectsInactive.Include);

				int mapWidth = gameOptions.GameOptions.MapSize.x;
				int mapHeight = gameOptions.GameOptions.MapSize.y;

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

				var scene = SceneManager.GetActiveScene();
				string sceneFullPath = Path.GetFullPath(scene.path);
				string directoryPath = sceneFullPath[..sceneFullPath.LastIndexOf('.')];

				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}

				string filePath = Path.Combine(directoryPath, $"{scene.name}.Minimap.png");

				Debug.Log(filePath);
				File.WriteAllBytes(filePath, data);
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
