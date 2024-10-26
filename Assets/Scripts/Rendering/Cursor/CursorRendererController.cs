using Omniverse.Abilities;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class CursorRendererController : ILateTickable
	{
		private CursorRendererConfig Config { get; }

		[Inject]
		private AbilityController AbilityController { get; set; }

		public CursorRendererController(CursorRendererConfig config)
		{
			Config = config;
		}

		public void LateTick()
		{
			CursorParams cursorTexture = GetCursorParams();

			Cursor.SetCursor(cursorTexture.Texture, cursorTexture.Hotspot, CursorMode.Auto);

			CursorParams GetCursorParams()
			{
				if (AbilityController.ActiveAbility == null)
				{
					return Config.Default;
				}

				switch (AbilityController.ActiveAbility.Desc.Target)
				{
					case UnitTarget:
						return Config.UnitTarget;
					default:
						return Config.Target;
				}
			}
		}
	}
}