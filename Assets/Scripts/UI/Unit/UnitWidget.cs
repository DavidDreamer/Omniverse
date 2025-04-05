using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class UnitWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Canvas Canvas { get; set; }

		[field: SerializeField]
		private AvatarWidget Avatar { get; set; }

		[field: SerializeField]
		private PropertyBarWidget Health { get; set; }

		[field: SerializeField]
		private PropertyBarWidget Mana { get; set; }

		[field: SerializeField]
		private ExperienceWidget Experience { get; set; }

		[field: SerializeField]
		private PropertiesWidget Properties { get; set; }

		[field: SerializeField]
		private AbilityBarWidget AbilityBar { get; set; }

		[field: SerializeField]
		private EffectsBarWidget EffectsBar { get; set; }

		[field: SerializeField]
		private ActionBarWidget ActionBar { get; set; }

		public void LateUpdate()
		{
			EntityManager entityManager = ECSUtils.ClientWorld.EntityManager;

			var selection = ECSUtils.GetSingleton<Selection>();

			bool hasSelection = selection.HasSelection;

			Canvas.enabled = hasSelection;

			if (hasSelection is false)
			{
				return;
			}

			Entity entity = selection.Entity;

			Avatar.Bind(entity);

			bool hasHealth = entityManager.HasComponent<Health>(entity);
			Health.gameObject.SetActive(hasHealth);
			if (hasHealth)
			{
				var health = entityManager.GetComponentData<Health>(entity);
				Health.Tick(health);
			}

			bool hasMana = entityManager.HasComponent<Mana>(entity);
			Mana.gameObject.SetActive(hasMana);
			if (hasMana)
			{
				var mana = entityManager.GetComponentData<Mana>(entity);
				Mana.Tick(mana);
			}

			AbilityBar.Tick(entity);

			//TODO ECS
			//EffectsBar.Bind(unit);
			//EffectsBar.Tick();

			//Experience.Bind(unit.Experience);
			//Properties.Bind(unit);

			ActionBar.Tick(entity);
		}
	}
}
