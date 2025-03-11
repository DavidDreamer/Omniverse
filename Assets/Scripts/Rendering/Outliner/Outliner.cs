using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class Outliner : IInitializable, ILateTickable
	{
		private Player Player { get; set; }

		public EntityDetector EntityDetector { get; private set; }

		[Inject]
		public OutlineRenderer OutlineRenderer { get; private set; }

		public void Initialize()
		{
			Player = ECSUtils.GetSingleton<Player>();
			EntityDetector = ECSUtils.GetSingleton<EntityDetector>();
		}

		public void LateTick()
		{
			OutlineRenderer.Clear();

			if (EntityDetector.Entity == Entity.Null)
			{
				return;
			}

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
		}

		private int GetOutlineVariant(OmniverseEntity entity)
		{
			if (entity.FactionID == -1)
			{
				return 2;
			}

			return entity.FactionID == Player.FactionID ? 0 : 1;
		}
	}
}
