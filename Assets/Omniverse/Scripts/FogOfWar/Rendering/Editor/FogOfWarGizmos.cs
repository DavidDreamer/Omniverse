using UnityEditor;
using UnityEngine;

namespace Omniverse.FogOfWar.Rendering.Editor
{
	public static class FogOfWarGizmos
	{
		private static Color Visible { get; } = Color.green;

		private static Color Invisible { get; } = Color.red;
		
		private static Color Occluded { get; } = Color.black;

		[DrawGizmo(GizmoType.Selected, typeof(FogOfWarObstacle))]
		private static void DrawObstacle(FogOfWarObstacle obstacle, GizmoType gizmoType)
		{
			Gizmos.DrawWireCube(obstacle.transform.position, obstacle.Size);
		}
		
		[DrawGizmo(GizmoType.Selected, typeof(FogOfWarRenderer))]
		private static void DrawGizmos(FogOfWarRenderer fogOfWarRenderer, GizmoType gizmoType)
		{
			if (!Application.isPlaying)
			{
				return;
			}

			float width = fogOfWarRenderer.Manager.Resolution.x;
			float height = fogOfWarRenderer.Manager.Resolution.y;

			var cellVisibilityStates = fogOfWarRenderer.Manager.CellsVisibilityPerFaction[0];
			var cellsObstacles = fogOfWarRenderer.Manager.CellsObstaclesPerFaction[0];
			
			Vector3 size = new Vector3(1, 0, 1) * Manager.Multiplier;

			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					int index = x * fogOfWarRenderer.Manager.Resolution.y + y;
					Vector3 position = Manager.CalculateCellCenter(x, y);
					Color color = cellsObstacles[index] ? Occluded :
						cellVisibilityStates[index] is CellVisibilityState.Visible ? Visible : Invisible;

					Gizmos.DrawWireCube(position, size);

					Gizmos.color = color;
					Gizmos.DrawCube(position, size);
					Gizmos.color = Color.white;
				}
			}
		}
	}
}
