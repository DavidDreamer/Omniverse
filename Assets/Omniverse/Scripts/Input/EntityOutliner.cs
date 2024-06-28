using Dreambox.Rendering.URP;
using Omniverse.Entities.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class EntityOutliner: ILateTickable
	{
		[Inject]
		private Player Player { get; set; }

		[Inject]
		public EntityDetector EntityDetector { get; private set; }

		[Inject]
		public OutlineRendererFeature Outline { get; private set; }

		public void LateTick()
		{
			Outline.Pass.Clear();

			EntityPresenter entityPresenter = EntityDetector.Target;

			if (entityPresenter != null)
			{
				foreach (Renderer renderer in entityPresenter.Renderers)
				{
					int variant = GetOutlineVariantByFactionID(entityPresenter);
					var outlineRenderer = new OutlineRenderer(renderer, variant);
					Outline.Pass.AddRenderer(outlineRenderer);
				}
			}
		}

		private int GetOutlineVariantByFactionID(EntityPresenter entityPresenter)
		{
			switch (entityPresenter)
			{
				case UnitPresenter unitPresenter:
					return unitPresenter.Entity.FactionID == Player.FactionID ? 0 : 1;
				default:
					return 2;
			}
		}
	}
}
