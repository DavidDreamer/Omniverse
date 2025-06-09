using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct OutlineRenderSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<Player>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var OutlineRenderer = Object.FindFirstObjectByType<OutlineRenderer>(FindObjectsInactive.Include);

			OutlineRenderer.Clear();

			var player = SystemAPI.GetSingleton<Player>();
			var entityDetector = SystemAPI.GetSingleton<Pointer>();
			var entity = entityDetector.Entity;

			if (entity == Entity.Null)
			{
				return;
			}

			if (SystemAPI.HasComponent<RenderMeshUnmanaged>(entity))
			{
				var materialMeshInfo = SystemAPI.GetComponent<MaterialMeshInfo>(entityDetector.Entity);

				int outlineVariant = 0;
				if (SystemAPI.HasComponent<Faction>(entity))
				{
					var faction = SystemAPI.GetComponent<Faction>(entity);
					outlineVariant = faction.ID == player.FactionID ? 0 : 1;
				}
				else
				{
					outlineVariant = -2;
				}

				//TODO ECS
				//foreach (Renderer renderer in entityRenderer.Renderers)
				//{
				//	var outlineTarget = new OutlineTarget(renderer, outlineVariant);
				//	outlinerRenderer.AddTarget(outlineTarget);
				//}
			}
		}
	}
}
