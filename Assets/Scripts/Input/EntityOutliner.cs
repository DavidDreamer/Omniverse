using Dreambox.Rendering.URP;
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

			Entity entity = EntityDetector.Target;

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
					var outlineRenderer = new OutlineRenderer(renderer, variant);
					Outline.Pass.AddRenderer(outlineRenderer);
				}
			}
		}

		private int GetOutlineVariant(Entity entity) =>
			entity switch
			{
				IFactious factious => factious.FactionID == Player.FactionID ? 0 : 1,
				_ => 2
			};
	}
}
