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
		private Player Player { get; set; }

		[Inject]
		private Detector Detector { get; set; }

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
					if (Detector.Target != null)
					{
						if (Detector.Target.IsAllyFor(Player))
						{
							return Config.HoverAlly;
						}
						else
						{
							return Config.HoverEnemy;
						}
					}

					return Config.Default;
				}
				else
				{
					switch (AbilityController.ActiveAbility.Desc.Target)
					{
						case UnitTarget:
							return Detector.Target is UnitObsolete ? Config.TargetUnit : Config.TargetInvalid;
						default:
							return Config.TargetDefault;
					}
				}
			}
		}
	}
}