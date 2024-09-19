using System;
using Dreambox.Core;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionRendererPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));
		}

		private SelectionRenderer Renderer { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] Colors { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public SelectionRendererPass(SelectionRenderer renderer)
		{
			Renderer = renderer;

			Matrices = new Matrix4x4[UnitSelector.Capacity];
			Colors = new Vector4[UnitSelector.Capacity];
			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public void Dispose()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "Selection");
			var commandBuffer = scope.CommandBuffer;

			SelectionRendererConfig config = Renderer.Config;
			UnitSelector unitSelector = Renderer.UnitSelector;

			MaterialPropertyBlock.Clear();

			for (int i = 0; i < unitSelector.SelectedUnits.Count; i++)
			{
				Unit unit = unitSelector.SelectedUnits[i];
				Matrices[i] = unit.transform.localToWorldMatrix * MatrixUtils.WorldUpRotation;
				Colors[i] = Renderer.Player.FactionID == unit.FactionID ?
					unitSelector.SelectedUnit == unit ? config.MainSelectionColor : config.AllyColor :
					config.EnemyColor;
			}

			MaterialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, Colors);

			var drawMeshParams = config.DrawMeshParams;
			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				unitSelector.SelectedUnits.Count,
				MaterialPropertyBlock);
		}
	}
}
