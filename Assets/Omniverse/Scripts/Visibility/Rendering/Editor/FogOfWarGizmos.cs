using UnityEditor;
using UnityEngine;

namespace Omniverse.Visibility.Rendering.Editor
{
	public static class FogOfWarGizmos
	{
		private static Color Visible { get; } = Color.green;

		private static Color Invisible { get; } = Color.red;

		[DrawGizmo(GizmoType.Selected, typeof(FogOfWarRenderer))]
		private static void DrawGizmos(FogOfWarRenderer fogOfWarRenderer, GizmoType gizmoType)
		{
			if (!Application.isPlaying)
			{
				return;
			}

			float width = fogOfWarRenderer.FogOfWar.Resolution.x;
			float height = fogOfWarRenderer.FogOfWar.Resolution.y;

			Vector3 size = new Vector3(1, 0, 1) * FogOfWar.Multiplier;

			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					FogOfWarCell cell = fogOfWarRenderer.FogOfWar.Cells[x, y];
					Vector3 position = cell.Position;
					Color color = cell.Reveled[fogOfWarRenderer.FogOfWar.Player.FactionID] ? Visible : Invisible;

					//Gizmos.DrawWireCube(cell.Position, size);

					Gizmos.color = color;
					Gizmos.DrawCube(position, size);
					Gizmos.color = Color.white;
				}
			}
		}
	}
}
