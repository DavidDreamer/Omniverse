using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UnitWidget : MonoBehaviour, IInitializable
	{
		[field: SerializeField]
		private Canvas Canvas { get; set; }

		[field: SerializeField]
		private AvatarWidget Avatar { get; set; }

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

		public PropertyBarWidget Health;

		public void Initialize()
		{
		}

		public void LateUpdate()
		{
			var selection = ECSUtils.GetSingleton<Selection>();

			bool hasSelection = selection.HasSelection;

			Canvas.enabled = hasSelection;

			if (hasSelection is false)
			{
				return;
			}

			Entity entity = selection.Entity;

			Avatar.Bind(entity);
			//TODO ECS
			//Health.Bind(unit);

			AbilityBar.Bind(entity);
			AbilityBar.Tick();

			//EffectsBar.Bind(unit);
			//EffectsBar.Tick();

			//Experience.Bind(unit.Experience);
			//Properties.Bind(unit);

			//ActionBar.Bind(unit);
			//ActionBar.Tick();
		}
	}
}
