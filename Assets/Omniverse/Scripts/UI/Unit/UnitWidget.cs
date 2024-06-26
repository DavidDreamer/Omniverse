using System.Linq;
using Omniverse.Input;
using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UnitWidget: MonoBehaviour, ILateTickable
	{
		[field: SerializeField]
		private Canvas Canvas { get; set; }
		
		[field: SerializeField]
		private ExperienceWidget Experience { get; set; }
		
		[field: SerializeField]
		private PropertiesWidget Properties { get; set; }
		
		[field: SerializeField]
		private AbilityBarWidget AbilityBar { get; set; }
		
		[field: SerializeField]
		private EffectsBarWidget EffectsBar { get; set; }
		
		public PropertyBarWidget Health;

		public Image Icon;
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }
		
		public void LateTick()
		{
			bool hasSelection = UnitSelector.SelectedUnits.Count > 0;

			Canvas.enabled = hasSelection;

			if (hasSelection is false)
			{
				return;
			}
			
			Unit unit = UnitSelector.SelectedUnit.Unit;

			Icon.sprite = unit.Desc.Presentation.Icon;
				
			Health.Bind(unit);

			AbilityBar.Bind(unit);
			AbilityBar.Tick();

			EffectsBar.Bind(unit);
			EffectsBar.Tick();
			
			Experience.Bind(unit.Experience);
			Properties.Bind(unit);
		}
	}
}
