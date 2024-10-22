using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ExperienceWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Slider Slider { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		private Experience Experience { get; set; }

		public void Bind(Experience experience)
		{
			Experience = experience;
		}

		public void LateUpdate()
		{
			if (Experience is null)
			{
				return;
			}

			Slider.minValue = Experience.PointsForCurrentLevel();
			Slider.maxValue = Experience.PointsForNextLevel();
			Slider.value = Experience.Points;
		}
	}
}
