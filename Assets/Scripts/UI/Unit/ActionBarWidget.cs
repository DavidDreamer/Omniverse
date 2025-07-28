using Omniverse.Abilities;
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

		public void Tick(EntityManager entityManager, Entity entity)
		{
			//TODO CASTING
			//var commandModule = entityManager.GetComponentObject<CommandModule>(entity);

			//CastAbilityCommand castAbilityCommand = commandModule.Command as CastAbilityCommand;

			//bool isCastAbilityCommand = castAbilityCommand != null;
			//gameObject.SetActive(isCastAbilityCommand);

			//if (!isCastAbilityCommand)
			//{
			//	return;
			//}

			//Entity ability = castAbilityCommand.AbilityEntity;
			//var metaData = entityManager.GetComponentData<MetaData>(ability);
			//var casting = entityManager.GetComponentData<Casting>(ability);

			//Icon.sprite = metaData.Icon;
			//Slider.minValue = 0f;
			//Slider.maxValue = casting.Time;
			//Slider.value = casting.CurrentTime;
		}
	}
}
