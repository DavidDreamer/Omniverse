using Omniverse.Abilities;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class CursorRenderSystem : SystemBase
	{
		private CursorRendererConfig Config { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<Player>();

			var rendering = Object.FindFirstObjectByType<RenderingClient>(FindObjectsInactive.Include);
			Config = rendering.CursorRendererConfig;
		}

		protected override void OnUpdate()
		{
			CursorParams cursorTexture = GetCursorParams();

			Cursor.SetCursor(cursorTexture.Texture, cursorTexture.Hotspot, CursorMode.Auto);

			CursorParams GetCursorParams()
			{
				var player = SystemAPI.GetSingleton<Player>();
				var pointer = SystemAPI.GetSingleton<Pointer>();
				var selection = SystemAPI.GetSingleton<Selection>();

				Entity entity = pointer.Entity;

				if (selection.AbilityInProcess)
				{
					var abiltiyTarget = EntityManager.GetComponentObject<AbilityTarget>(selection.Ability);
					switch (abiltiyTarget.Target)
					{
						case UnitTarget:
							return EntityManager.HasComponent<Unit>(entity) ? Config.TargetUnit : Config.TargetInvalid;
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
							if (EntityManager.HasComponent<Faction>(entity))
							{
								var faction = EntityManager.GetComponentData<Faction>(entity);
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