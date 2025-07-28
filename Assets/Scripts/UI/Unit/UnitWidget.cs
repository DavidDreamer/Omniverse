using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class UnitWidget : Widget
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

		public override void Tick()
		{
			var selection = EntityManager.GetSingleton<Selection>();

			bool hasSelection = selection.HasSelection;

			Canvas.enabled = hasSelection;

			if (hasSelection is false)
			{
				return;
			}

			Entity entity = selection.Entity;

			Avatar.Bind(entity);

			bool hasHealth = EntityManager.HasComponent<Health>(entity);
			Health.gameObject.SetActive(hasHealth);
			if (hasHealth)
			{
				var health = EntityManager.GetComponentData<Health>(entity);
				Health.Tick(health);
			}

			bool hasMana = EntityManager.HasComponent<Mana>(entity);
			Mana.gameObject.SetActive(hasMana);
			if (hasMana)
			{
				var mana = EntityManager.GetComponentData<Mana>(entity);
				Mana.Tick(mana);
			}

			AbilityBar.Tick(EntityManager, entity);

			if (EntityManager.HasBuffer<Effect>(entity))
			{
				EffectsBar.gameObject.SetActive(false);

				var effects = EntityManager.GetBuffer<Effect>(entity);
				EffectsBar.Tick(effects);
			}
			else
			{
				EffectsBar.gameObject.SetActive(false);
			}

			//TODO ECS
			//Experience.Bind(unit.Experience);
			//Properties.Bind(unit);

			//if (EntityManager.HasComponent<CommandModule>(entity))
			//{
			//	ActionBar.Tick(EntityManager, entity);
			//}
		}
	}
}
