using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Visibility
{
	public class FogOfWarCell
	{
		public Vector3 Position;

		public bool[] Reveled;

		public float Value;
	}

	public class FogOfWar: IInitializable, IFixedTickable
	{
		public static int Multiplier { get; } = (int)Mathf.Pow(2f, 1f);
		
		public Vector2Int Resolution { get; set; }

		[Inject]
		private GameSettings GameSettings { get; set; }

		[Inject]
		public Player Player { get; set; }

		public FogOfWarCell[,] Cells { get; set; }

		public void Initialize()
		{
			Resolution = GameSettings.MapSettings.Size / Multiplier;
			
			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;

			Cells = new FogOfWarCell[Resolution.x, Resolution.y];
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					Cells[x, y] = new FogOfWarCell
					{
						Position = new Vector3(x, 0, y) * Multiplier + offset,
						Reveled = new bool[GameSettings.Factions.Length]
					};
				}
			}
		}

		public float delta = 0.03f;
		
		public void FixedTick()
		{
			var agents = UnityEngine.Object.FindObjectsOfType<FogOfWarAgent>();

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

					for (var i = 0; i < cell.Reveled.Length; i++)
					{
						cell.Reveled[i] = false;
					}
					
					foreach (FogOfWarAgent agent in agents)
					{
						float distance = (agent.Cell.Position - cell.Position).sqrMagnitude;
						
						cell.Reveled[agent.FactionID] |= distance <= agent.Range * agent.Range;
					}

					cell.Value += (cell.Reveled[Player.FactionID] ? -1 : 1) * delta;
					cell.Value = Mathf.Clamp01(cell.Value);
				}
			}
			
			foreach (FogOfWarAgent agent in agents)
			{
				agent.GetComponentInChildren<UnitRendererBase>(true).gameObject.SetActive(agent.Cell.Reveled[Player.FactionID]);
			}
		}
	}
}
