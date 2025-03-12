using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using Unity.Rendering;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class Outliner : ILateTickable
	{
		[Inject]
		public OutlineRenderer OutlineRenderer { get; private set; }

		public void LateTick()
		{
			OutlineRenderer.Clear();

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
			//		OutlineRenderer.AddTarget(outlineTarget);
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
