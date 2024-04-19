using System.Linq;
using Omniverse.Camera;
using UnityEngine;
using VContainer;

namespace Omniverse.UI
{
	//TEMP
	public class UnitAvatarWidget: MonoBehaviour
	{
		public PropertyTag HealthTag;
		
		public PropertyBarWidget Health;
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		public void LateUpdate()
		{
			if (UnitSelector.SelectedUnits.Count > 0)
			{
				Unit unit = UnitSelector.SelectedUnits.First();
				
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
