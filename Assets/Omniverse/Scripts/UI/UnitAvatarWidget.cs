using System.Linq;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	//TEMP
	public class UnitAvatarWidget: MonoBehaviour
	{
		public PropertyTag HealthTag;
		
		public PropertyBarWidget Health;

		public Image Icon;
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		public void LateUpdate()
		{
			if (UnitSelector.SelectedUnits.Count > 0)
			{
				Unit unit = UnitSelector.SelectedUnits.First().Unit;

				Icon.sprite = unit.Desc.Presentation.Icon;
				
				if (unit.Properties.TryGetValue(HealthTag, out Property property))
				{
					Health.Bind(property);
				}
			}
			else
			{
				Health.Unbind();
			}
		}
	}
}
