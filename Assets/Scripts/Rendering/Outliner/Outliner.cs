using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
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

			if (entityDetector.Entity == Entity.Null)
			{
				return;
			}

			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			//var materialMeshInfo = entityManager.GetComponentData<MaterialMeshInfo>(entityDetector.Entity);

			//EnEntity
			////TODO
			//var entityRenderer = entity.GetComponentInChildren<IRendererComponent>();
			//if (entityRenderer != null)
			//{
			//	foreach (Renderer renderer in entityRenderer.Renderers)
			//	{
			//		int variant = GetOutlineVariant(entity);
			//		var outlineTarget = new OutlineTarget(renderer, variant);
			//		outlinerRenderer.AddTarget(outlineTarget);
			//	}
			//}

			//int GetOutlineVariant(OmniverseEntity entity)
			//{
			//	if (entity.FactionID == -1)
			//	{
			//		return 2;
			//	}

			//	return entity.FactionID == player.FactionID ? 0 : 1;
			//}
		}
	}
}
