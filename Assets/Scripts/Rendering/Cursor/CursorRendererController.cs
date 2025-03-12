using System;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class CursorRendererController : ILateTickable
	{
		private CursorRendererConfig Config { get; }

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
				var entityDetector = ECSUtils.GetSingleton<Pointer>();
				var abilityInput = ECSUtils.GetSingleton<AbilityInput>();

				EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
				Entity entity = entityDetector.Entity;

				if (abilityInput.Ability == Entity.Null)
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
					return Config.TargetDefault;

					//TODO ECS
					//switch (AbilityController.ActiveAbility.Desc.Target)
					//{
					//	case UnitTarget:
					//		return entityManager.HasComponent<Unit>(entity) ? Config.TargetUnit : Config.TargetInvalid;
					//	default:
					//		return Config.TargetDefault;
					//}
				}
			}
		}
	}
}