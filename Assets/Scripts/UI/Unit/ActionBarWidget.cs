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

		public void Tick(Entity entity)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var commandModule = entityManager.GetComponentObject<CommandModule>(entity);

			CastAbilityCommand castAbilityCommand = commandModule.Command as CastAbilityCommand;

			bool isCastAbilityCommand = castAbilityCommand != null;
			gameObject.SetActive(isCastAbilityCommand);

			if (!isCastAbilityCommand)
			{
				return;
			}

			Icon.sprite = castAbilityCommand.Ability.Icon;
			Slider.minValue = 0f;
			Slider.maxValue = castAbilityCommand.Ability.Casting.Time;
			Slider.value = castAbilityCommand.Ability.Casting.CurrentTime;
		}
	}
}
