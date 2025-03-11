using Omniverse.Abilities;
using Omniverse.Input;
using Unity.Entities;
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
				var player = ECSUtils.GetSingleton<Player>();
				var entityDetector = ECSUtils.GetSingleton<EntityDetector>();
				EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
				Entity entity = entityDetector.Entity;

				if (AbilityController.ActiveAbility == null)
				{
					if (entity != Entity.Null)
					{
						if (entityManager.HasComponent<Faction>(entity))
						{
							var faction = entityManager.GetComponentData<Faction>(entity);
							if (faction.ID == player.FactionID)
							{
								return Config.HoverAlly;
							}
							else
							{
								return Config.HoverEnemy;
							}
						}
				
					}

					return Config.Default;
				}
				else
				{
					switch (AbilityController.ActiveAbility.Desc.Target)
					{
						case UnitTarget:
							return entityManager.HasComponent<Unit>(entity) ? Config.TargetUnit : Config.TargetInvalid;
						default:
							return Config.TargetDefault;
					}
				}
			}
		}
	}
}