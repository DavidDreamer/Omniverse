using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Visibility
{
	// public static class Bresenham
	// {
	// 	public static IEnumerable<(int, int)> Line(int x0, int x1, int y0, int y1)
	// 	{
	// 		
	// 	}
	// }
	
	public class FogOfWarCell
	{
		public List<FogOfWarCell> Neighbours;

		public bool Occluded;

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
						Reveled = new bool[GameSettings.Factions.Length],
						Neighbours = new()
					};
				}
			}

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					var cell = Cells[x, y];
					if (x > 0)
					{
						cell.Neighbours.Add(Cells[x - 1, y]);
					}

					if (x < Resolution.x - 1)
					{
						cell.Neighbours.Add(Cells[x + 1, y]);
					}
					
					if (y > 0)
					{
						cell.Neighbours.Add(Cells[x, y - 1]);
					}

					if (y < Resolution.y - 1)
					{
						cell.Neighbours.Add(Cells[x, y + 1]);
					}
				}
			}
		}

		public float delta = 0.03f;

		struct Temp
		{
			public FogOfWarCell Cell;

			public int Steps;
		}
		
		private Queue<FogOfWarCell> Queue { get; } = new();
		private HashSet<FogOfWarCell> Set { get; } = new();
		private Dictionary<FogOfWarCell, int> Dictionary { get; } = new();

		public void AddObstacle(FogOfWarObstacle obstacle)
		{
			FogOfWarCell cell = CalculateCell(obstacle.transform);
			
			Queue.Clear();
			Set.Clear();

			Queue.Enqueue(cell);
			Set.Add(cell);

			while (Queue.Count > 0)
			{
				cell = Queue.Dequeue();

				Vector3 cellPosition = obstacle.transform.InverseTransformPoint(cell.Position);

				bool contains = Mathf.Abs(cellPosition.x) <= obstacle.Size.x / 2f &&
				                Mathf.Abs(cellPosition.z) <= obstacle.Size.z / 2f;
				if (!contains)
				{
					continue;
				}
				
				cell.Occluded = true;

				foreach (FogOfWarCell neighbourCell in cell.Neighbours)
				{
					if (!Set.Contains(neighbourCell))
					{
						Set.Add(neighbourCell);
						Queue.Enqueue(neighbourCell);
					}
				}
			}
		}

		private FogOfWarCell CalculateCell(Transform transform)
		{
			Vector3 position = transform.position;

			int x = (int)position.x / Multiplier;
			int y = (int)position.z / Multiplier;

			return Cells[x, y];
		}
		
		private FogOfWarAgent[] Agents {get;set;}
			
		private void UpdateAgentPositions()
		{
			Agents = Object.FindObjectsOfType<FogOfWarAgent>();

			foreach (FogOfWarAgent agent in Agents)
			{
				agent.Cell = CalculateCell(agent.transform);
			}
		}

		private void UpddateAgentsVisibility()
		{
			foreach (FogOfWarAgent agent in Agents)
			{
				agent.GetComponentInChildren<UnitRendererBase>(true).gameObject
					.SetActive(agent.Cell.Reveled[Player.FactionID]);
			}
		}
		
		private void Clear()
		{
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					for (var i = 0; i < cell.Reveled.Length; i++)
					{
						cell.Reveled[i] = false;
					}
				}
			}
		}

		private void Animate()
		{
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					//cell.Reveled[Player.FactionID] = Set.Contains(cell) && (!cell.Occluded || cell.Block[Player.FactionID] > cell.Vision[Player.FactionID]);
					
					cell.Value += (cell.Reveled[Player.FactionID] ? -1 : 1) * delta;
					cell.Value = Mathf.Clamp01(cell.Value);
				}
			}
		}

		private void CalculateVisibility(int x0, int y0, int x1, int y1, int faction = 0)
		{
			// Cells[x1, y1].Reveled[faction] = true;
			// Cells[x1 + 1, y1].Reveled[faction] = true;
			// return;
			// float slope = (y1 - y0) / (float)(x1 - x0);
			//
			// if (slope > 1 || slope < 0)
			// {
			// 	return;
			// }
			//
			// int deltaX = Mathf.Abs(x1 - x0);
			// int deltaY = Mathf.Abs(y1 - y0);
			// int signX = x0 < x1 ? 1 : -1;
			// int signY = y0 < y1 ? 1 : -1;
			//
			// int error = deltaX - deltaY;
			//
			// Cells[x, y].Reveled[faction] = true;
			// while(x1 != x2 || y1 != y2) 
			// {
			// 	setPixel(x1, y1);
			// 	int error2 = error * 2;
			// 	if(error2 > -deltaY) 
			// 	{
			// 		error -= deltaY;
			// 		x1 += signX;
			// 	}
			// 	if(error2 < deltaX) 
			// 	{
			// 		error += deltaX;
			// 		y1 += signY;
			// 	}
			// }
			
			// var steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0); // Проверяем рост отрезка по оси икс и по оси игрек
			// // Отражаем линию по диагонали, если угол наклона слишком большой
			// if (steep)
			// {
			// 	(x0, y0) = (y0, x0); // Перетасовка координат вынесена в отдельную функцию для красоты
			// 	(x1, y1) = (y1, x1); // Перетасовка координат вынесена в отдельную функцию для красоты
			// }
			// // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
			// if (x0 > x1)
			// {
			// 	(x0, x1) = (x1, x0);
			// 	(y0, y1) = (y1, y0);
			// }
			// int dx = x1 - x0;
			// int dy = Mathf.Abs(y1 - y0);
			// int error = dx / 2; // Здесь используется оптимизация с умножением на dx, чтобы избавиться от лишних дробей
			// int ystep = (y0 < y1) ? 1 : -1; // Выбираем направление роста координаты y
			// int y = y0;
			// for (int x = x0; x <= x1; x++)
			// {
			// 	var cell = Cells[steep ? y : x, steep ? x : y]; // Не забываем вернуть координаты на место
			//
			// 	if (cell.Occluded)
			// 	{
			// 		return;
			// 	}
			// 	
			// 	cell.Reveled[faction] = true;
			// 	error -= dy;
			// 	if (error < 0)
			// 	{
			// 		y += ystep;
			// 		error += dx;
			// 	}
			// }

			int dx = x1 - x0;
			int dy = y1 - y0;
			
			int incx = (int)Mathf.Sign(dx);
			int incy = (int)Mathf.Sign(dy);
			
			dx = Mathf.Abs(dx);
			dy = Mathf.Abs(dy);
			
			int pdx, pdy;
			int es, el;
			
			if (dx > dy)
			{
				pdx = incx;
				pdy = 0;
				es = dy;
				el = dx;
			}
			else
			{
				pdx = 0;
				pdy = incy;
				es = dx;
				el = dy;
			}
			
			int x = x0;
			int y = y0;
			
			int error = el / 2;
			
			Cells[x, y].Reveled[faction] = true;
			
			for (int t = 0; t < el; ++t)
			{
				error -= es;
				if (error < 0)
				{
					error += el;
					x += incx;
					y += incy;
				}
				else
				{
					x += pdx;
					y += pdy;
				}
				
				if (Cells[x, y].Occluded)
				{
					return;
				}
				
				Cells[x, y].Reveled[faction] = true;
			}
		}

		private void Calc(int x0, int y0, int radius)
		{
			int x = radius;
			int y = 0;
			int radiusError = 1 - x;
			
			while (x >= y)
			{
				CalculateVisibility(x0, y0, x0 + x, y0 + y);
				CalculateVisibility(x0, y0, x0 + x, y0 - y);
				CalculateVisibility(x0, y0, x0 - x, y0 + y);
				CalculateVisibility(x0, y0, x0 - x, y0 - y);
				CalculateVisibility(x0, y0, x0 + y, y0 + x);
				CalculateVisibility(x0, y0, x0 + y, y0 - x);
				CalculateVisibility(x0, y0, x0 - y, y0 + x);
				CalculateVisibility(x0, y0, x0 - y, y0 - x);

				if (true)
				{
					CalculateVisibility(x0, y0, x0 + x + 1, y0 + y);
					CalculateVisibility(x0, y0, x0 + x + 1, y0 - y);
					CalculateVisibility(x0, y0, x0 - x + 1, y0 + y);
					CalculateVisibility(x0, y0, x0 - x + 1, y0 - y);
					CalculateVisibility(x0, y0, x0 + y + 1, y0 + x);
					CalculateVisibility(x0, y0, x0 + y + 1, y0 - x);
					CalculateVisibility(x0, y0, x0 - y + 1, y0 + x);
					CalculateVisibility(x0, y0, x0 - y + 1, y0 - x);	
				}
				
				y++;

				if (radiusError < 0)
				{
					radiusError += 2 * y + 1;
				}
				else
				{
					x--;
					radiusError += 2 * (y - x + 1);
				}
			}
		}
		
		private void CalculateVisibility()
		{
			foreach (FogOfWarAgent agent in Agents)
			{
				int x0 = (int)agent.transform.position.x / Multiplier;
				int y0 = (int)agent.transform.position.z / Multiplier;
				int radius = (int)agent.Range / Multiplier;
				
				Calc(x0, y0, radius);
			}
		}
		
		public void FixedTick()
		{
			UpdateAgentPositions();
			Clear();
			
			CalculateVisibility();

			Animate();

			UpddateAgentsVisibility();
		}
	}
}
