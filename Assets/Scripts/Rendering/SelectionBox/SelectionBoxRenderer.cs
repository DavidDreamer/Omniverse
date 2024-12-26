using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;
using VContainer;

namespace Omniverse.Rendering
{
	public class SelectionBoxRenderer : CustomRenderer<SelectionBoxRendererConfig, SelectionBoxRendererPass>
	{
		[Inject]
		public Selector Selector { get; private set; }

		private Material Material { get; set; }

		protected override SelectionBoxRendererPass Setup(SelectionBoxRendererConfig config)
		{
			Material = config.Material;
			var pass = new SelectionBoxRendererPass(Material);
			return pass;
		}

		protected override bool IsInactive() => !Selector.InProcess;

		private void LateUpdate()
		{
			if (!Selector.InProcess)
			{
				return;
			}

			float x, z, y, w;

			if (Selector.StartPosition.x > Selector.EndPosition.x)
			{
				x = Selector.EndPosition.x;
				z = Selector.StartPosition.x;
			}
			else
			{
				x = Selector.StartPosition.x;
				z = Selector.EndPosition.x;
			}

			if (Selector.StartPosition.y > Selector.EndPosition.y)
			{
				y = Selector.EndPosition.y;
				w = Selector.StartPosition.y;
			}
			else
			{
				y = Selector.StartPosition.y;
				w = Selector.EndPosition.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			Material.SetVector(SelectionBoxShaderVariables.SelectionBox, selectionBox);
		}
	}
}
