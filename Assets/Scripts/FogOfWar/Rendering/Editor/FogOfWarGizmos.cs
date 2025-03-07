using UnityEditor;
using UnityEngine;

namespace Omniverse.Rendering.Editor
{
	public static class FogOfWarGizmos
	{
		private static Color Visible { get; } = Color.green;

		private static Color Invisible { get; } = Color.red;

		private static Color Occluded { get; } = Color.black;

		[DrawGizmo(GizmoType.Selected, typeof(FogOfWarObstacleAuthoring))]
		private static void DrawObstacle(FogOfWarObstacleAuthoring obstacle, GizmoType gizmoType)
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

			float width = fogOfWarRenderer.FogOfWar.Resolution.x;
			float height = fogOfWarRenderer.FogOfWar.Resolution.y;

			var cellVisibilityStates = fogOfWarRenderer.FogOfWar.CellsVisibilityPerFaction[0];
			var cellsObstacles = fogOfWarRenderer.FogOfWar.CellsObstaclesPerFaction[0];

			Vector3 size = new Vector3(1, 0, 1) * FogOfWarObsolete.Multiplier;

			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					int index = x * fogOfWarRenderer.FogOfWar.Resolution.y + y;
					Vector3 position = FogOfWarObsolete.CalculateCellCenter(x, y);
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
