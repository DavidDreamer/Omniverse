using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class SelectionBoxRenderer : CustomRenderer<SelectionBoxRendererConfig, SelectionBoxRendererPass>
	{
		private Material Material { get; set; }

		protected override SelectionBoxRendererPass Setup()
		{
			Material = Config.Material;
			var pass = new SelectionBoxRendererPass(Material);
			return pass;
		}

		protected override bool IsInactive() => !ECSUtils.GetSingleton<Selection>().InProcess;

		private void LateUpdate()
		{
			var selection = ECSUtils.GetSingleton<Selection>();

			if (!selection.InProcess)
			{
				return;
			}

			float x, z, y, w;

			if (selection.StartPosition.x > selection.EndPosition.x)
			{
				x = selection.EndPosition.x;
				z = selection.StartPosition.x;
			}
			else
			{
				x = selection.StartPosition.x;
				z = selection.EndPosition.x;
			}

			if (selection.StartPosition.y > selection.EndPosition.y)
			{
				y = selection.EndPosition.y;
				w = selection.StartPosition.y;
			}
			else
			{
				y = selection.StartPosition.y;
				w = selection.EndPosition.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			Material.SetVector(SelectionBoxShaderVariables.SelectionBox, selectionBox);
		}
	}
}
