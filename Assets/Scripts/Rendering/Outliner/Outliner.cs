using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class Outliner : ILateTickable
	{
		[Inject]
		private Player Player { get; set; }

		[Inject]
		public Detector Detector { get; private set; }

		[Inject]
		public OutlineRenderer OutlineRenderer { get; private set; }

		public void LateTick()
		{
			OutlineRenderer.Clear();

			OmniverseEntity entity = Detector.Target;

			if (entity == null)
			{
				return;
			}

			//TODO
			var entityRenderer = entity.GetComponentInChildren<IRendererComponent>();
			if (entityRenderer != null)
			{
				foreach (Renderer renderer in entityRenderer.Renderers)
				{
					int variant = GetOutlineVariant(entity);
					var outlineTarget = new OutlineTarget(renderer, variant);
					OutlineRenderer.AddTarget(outlineTarget);
				}
			}
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
