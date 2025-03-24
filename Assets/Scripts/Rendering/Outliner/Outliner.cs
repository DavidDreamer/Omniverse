using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class Outliner : MonoBehaviour
	{
		public void LateUpdate()
		{
			var outlinerRenderer = Object.FindFirstObjectByType<OutlineRenderer>();

			outlinerRenderer.Clear();

			var player = ECSUtils.GetSingleton<Player>();
			var entityDetector = ECSUtils.GetSingleton<Pointer>();
			var entity = entityDetector.Entity;

			if (entity == Entity.Null)
			{
				return;
			}

			EntityManager entityManager = ECSUtils.ClientWorld.EntityManager;

			if (entityManager.HasComponent<MaterialMeshInfo>(entity))
			{
				var materialMeshInfo = entityManager.GetComponentData<MaterialMeshInfo>(entityDetector.Entity);

				int outlineVariant = 0;
				if (entityManager.HasComponent<Faction>(entity))
				{
					var faction = entityManager.GetComponentData<Faction>(entity);
					outlineVariant = faction.ID == player.FactionID ? 0 : 1;
				}
				else
				{
					outlineVariant = -2;
				}

				//TODO ECS
				//foreach (Renderer renderer in entityRenderer.Renderers)
				//{
				//	var outlineTarget = new OutlineTarget(renderer, variant);
				//	outlinerRenderer.AddTarget(outlineTarget);
				//}
			}
		}
	}
}
