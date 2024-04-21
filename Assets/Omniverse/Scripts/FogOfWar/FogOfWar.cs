using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

namespace Omniverse.Camera
{
	public class FogOfWarCell
	{
		public Vector3 Position;

		public bool Reveled;

		public float Value;
	}

	public class FogOfWar: MonoBehaviour
	{
		private static int Multiplier { get; } = (int)Mathf.Pow(2f, 2f);
		
		private Vector2Int Resolution { get; set; }

		[Inject]
		private MapSettings MapSettings { get; set; }
		
		public Texture2D texture;

		public RenderTexture RenderTexture;
		public RenderTexture RenderTexture2;

		public Material Material;

		private Material BlurMaterial;

		private FogOfWarCell[,] Cells { get; set; }

		public float alpah;
		
		private void Awake()
		{
			Resolution = MapSettings.Size / Multiplier;
			
			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;

			Cells = new FogOfWarCell[Resolution.x, Resolution.y];
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					Cells[x, y] = new FogOfWarCell
					{
						Position = new Vector3(x, 0, y) * Multiplier + offset
					};
				}
			}

			texture = new Texture2D(Resolution.x, Resolution.y);

			Material.SetTexture("_BaseMap", RenderTexture2);

			BlurMaterial = new Material(Shader.Find("Hidden/Dreambox/Blur"));
			BlurMaterial.SetFloat("Factor", 1);
			BlurMaterial.SetKeyword(new LocalKeyword(BlurMaterial.shader, "ALGORITHM_GAUSSIAN"), true);
			
		}

		public float Radius;

		public float delta;
		
		public void Update()
		{
			BlurMaterial.SetFloat("Radius", Radius);
			
			var agents = FindObjectsOfType<FogOfWarAgent>();

			foreach (FogOfWarAgent agent in agents)
			{
				int x = Mathf.FloorToInt(agent.transform.position.x);
				int y = Mathf.FloorToInt(agent.transform.position.z);

				var cellindex = new Vector2Int(x, y) / Multiplier;
				agent.Cell = Cells[cellindex.x, cellindex.y];
			}

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					cell.Reveled = false;
					foreach (FogOfWarAgent agent in agents)
					{
						float distance = (agent.Cell.Position - cell.Position).sqrMagnitude;

						cell.Reveled |= distance <= agent.Range * agent.Range;
					}

					cell.Value += (cell.Reveled ? -1 : 1) * delta;
					cell.Value = Mathf.Clamp01(cell.Value);
					
					Color color = new Color(0, 0, 0, (cell.Value) * alpah);
					
					texture.SetPixel(x, y, color);
				}
			}
			
			texture.Apply();

			if (ApplyBlur)
			{
				Graphics.Blit(texture, RenderTexture, BlurMaterial, 0);
				Graphics.Blit(RenderTexture, RenderTexture2, BlurMaterial, 1);
			}
			else
			{
				Graphics.Blit(texture, RenderTexture2);
			}
		}

		public bool ApplyBlur;
		private void OnDrawGizmosSelected()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			Vector3 size = new Vector3(1, 0, 1) * Multiplier;

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					Vector3 position = cell.Position;
					Gizmos.DrawWireCube(cell.Position, size);

					Gizmos.color = cell.Reveled ? Color.green : Color.red;
					Gizmos.DrawCube(position, size);
					Gizmos.color = Color.white;
				}
			}
		}
	}
}
