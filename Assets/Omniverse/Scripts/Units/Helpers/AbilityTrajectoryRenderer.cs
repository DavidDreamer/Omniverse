using Dreambox.Math;
using UnityEngine;

namespace Bonecrusher.Abilities
{
	public class AbilityTrajectoryRenderer: MonoBehaviour
	{
		[field: SerializeField]
		private LineRenderer LineRenderer { get; set; }

		[field: SerializeField]
		[field: Range(0.01f, 100f)]
		private float Step { get; set; }

		public void Refresh(ParabolicTrajectory3D projectileTrajectory)
		{
			LineRenderer.positionCount = Mathf.RoundToInt(projectileTrajectory.Parameters.Time / Step);

			for (int i = 0; i < LineRenderer.positionCount; ++i)
			{
				float factor = (float)i / (LineRenderer.positionCount - 1);

				Vector3 position = projectileTrajectory.EvaluatePosition(factor);

				LineRenderer.SetPosition(i, position);
			}
		}
	}
}
