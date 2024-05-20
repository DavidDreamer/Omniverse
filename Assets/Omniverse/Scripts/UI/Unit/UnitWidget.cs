using System.Linq;
using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UnitWidget: MonoBehaviour, ILateTickable
	{
		[field: SerializeField]
		private ExperienceWidget Experience { get; set; }
		
		[field: SerializeField]
		private StatsWidget Stats { get; set; }
		
		public PropertyTag HealthTag;
		
		public PropertyBarWidget Health;

		public Image Icon;
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		public void LateTick()
		{
			if (UnitSelector.SelectedUnits.Count > 0)
			{
				Unit unit = UnitSelector.SelectedUnits.First().Unit;

				Icon.sprite = unit.Desc.Presentation.Icon;
				
				if (unit.Properties.TryGetValue(HealthTag, out Property property))
				{
					Health.Bind(property);
				}

				Experience.Bind(unit.Experience);
				Stats.Bind(unit);
			}
			else
			{
				Health.Unbind();
			}
		}
	}
}
