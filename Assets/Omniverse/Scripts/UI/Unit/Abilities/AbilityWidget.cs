using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class AbilityWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Activator { get; set; }

		[field: SerializeField]
		private CooldownWidget Cooldown { get; set; }

		[field: SerializeField]
		private HotkeyWidget Hotkey { get; set; }
		
		[field: SerializeField]
		private Material DefaultMaterial { get; set; }
		
		[field: SerializeField]
		private Material OnCooldownMaterial { get; set; }
		
		private Ability Ability { get; set; }

		public void Bind(Ability ability, InputAction inputAction)
		{
			Ability = ability;

			Icon.sprite = ability.Desc.Presentation.Icon;
			Hotkey.Set(inputAction);

			if (Ability.Cooldown is not null)
			{
				Cooldown.Bind(Ability.Cooldown);
			}
		}

		public void Unbind()
		{
			Ability = default;
		}

		public void Tick()
		{
			Activator.enabled = Ability.AwaitsTarget;
			
			if (Ability.Cooldown is not null)
			{
				Icon.material = Ability.Cooldown.IsActive ? OnCooldownMaterial : DefaultMaterial;
				Cooldown.Tick();
			}
		}
	}
}
