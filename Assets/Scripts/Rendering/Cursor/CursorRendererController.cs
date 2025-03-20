using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class CursorRendererController : MonoBehaviour
	{
		[field: SerializeField]
		private CursorRendererConfig Config { get; set; }

		public void LateUpdate()
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

				if (!abilityInput.InProcess)
				{
					if (entity != Entity.Null)
					{
						if (entityManager.HasComponent<Faction>(entity))
						{
							var faction = entityManager.GetSharedComponent<Faction>(entity);
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