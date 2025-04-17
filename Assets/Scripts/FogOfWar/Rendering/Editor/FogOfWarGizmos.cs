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

			int width = fogOfWarRenderer.FogOfWar.Size.x;
			int height = fogOfWarRenderer.FogOfWar.Size.y;

			var cellVisibilityStates = fogOfWarRenderer.FogOfWar.Visibility;
			var cellsObstacles = fogOfWarRenderer.FogOfWar.Occlusion;

			Vector3 size = new Vector3(1, 0, 1) * FogOfWar.Multiplier;

			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					int index = x * fogOfWarRenderer.FogOfWar.Size.y + y;
					//TODO
					Vector3 halfMapSize = new(16, 0, 16);
					Vector3 position = (new Vector3(x, 0, y) * FogOfWar.Multiplier + size / 2f - halfMapSize);
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
