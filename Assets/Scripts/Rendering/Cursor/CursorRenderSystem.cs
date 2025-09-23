using Omniverse.Abilities;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class CursorRenderSystem : SystemBase
	{
		protected override void OnCreate()
		{
			RequireForUpdate<Player>();
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnUpdate()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			CursorRenderSettings settings = renderSettings.Cursor.Value;

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
					var abilityBuffer = SystemAPI.GetBuffer<Ability>(selection.Entity);
					Ability ability = abilityBuffer[selection.AbilityIndex];

					switch (ability.Desc.Value.Target)
					{
						case UnitTarget:
							return EntityManager.HasComponent<Unit>(entity) ? settings.TargetUnit : settings.TargetInvalid;
						default:
							return settings.TargetDefault;
					}
				}
				else
				{
					switch (pointer.TargetType)
					{
						case PointerTargetType.Entity:
						{
							if (SystemAPI.HasComponent<Faction>(entity))
							{
								var faction = SystemAPI.GetComponent<Faction>(entity);
								if (faction.ID == player.FactionID)
								{
									return settings.HoverAlly;
								}
								else
								{
									return settings.HoverEnemy;
								}
							}
							else
							{
								return settings.Default;
							}
						}
						default:
							return settings.Default;
					}
				}
			}
		}
	}
}