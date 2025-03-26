using Omniverse.Abilities;
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
				var pointer = ECSUtils.GetSingleton<Pointer>();
				var abilityInput = ECSUtils.GetSingletonManaged<AbilityInput>();

				EntityManager entityManager = ECSUtils.ClientWorld.EntityManager;
				Entity entity = pointer.Entity;

				if (abilityInput.InProcess)
				{
					var abiltiyTarget = entityManager.GetComponentObject<AbilityTarget>(abilityInput.Ability);
					switch (abiltiyTarget.Target)
					{
						case UnitTarget:
							return entityManager.HasComponent<Unit>(entity) ? Config.TargetUnit : Config.TargetInvalid;
						default:
							return Config.TargetDefault;
					}
				}
				else
				{
					switch (pointer.TargetType)
					{
						case PointerTargetType.Entity:
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
							else
							{
								return Config.Default;
							}
						}
						default:
							return Config.Default;
					}
				}
			}
		}
	}
}