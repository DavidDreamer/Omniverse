using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ActionBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Slider Slider { get; set; }

		//TODO ECS
		public void Tick(Entity entity)
		{
			CastAbilityCommand castAbilityCommand = null;// entity.CommandModule.Command as CastAbilityCommand;

			bool isCastAbilityCommand = false;// castAbilityCommand != null;
			gameObject.SetActive(isCastAbilityCommand);

			if (!isCastAbilityCommand)
			{
				return;
			}

			Icon.sprite = castAbilityCommand.Ability.Desc.Meta.Icon;
			Slider.minValue = 0f;
			Slider.maxValue = castAbilityCommand.Ability.Desc.Casting.Time;
			Slider.value = castAbilityCommand.Ability.Casting.Time;
		}
	}
}
