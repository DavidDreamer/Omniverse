using Dreambox.Rendering.Core;
using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));
		}

		private SelectionRenderer Renderer { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] Colors { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public SelectionPass(SelectionRenderer renderer)
		{
			Renderer = renderer;

			Matrices = new Matrix4x4[UnitSelector.Capacity];
			Colors = new Vector4[UnitSelector.Capacity];
			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "Selection");
			var commandBuffer = scope.CommandBuffer;

			SelectionConfig config = Renderer.Config;
			UnitSelector unitSelector = Renderer.UnitSelector;

			MaterialPropertyBlock.Clear();

			for (int i = 0; i < unitSelector.SelectedUnits.Count; i++)
			{
				Unit unit = unitSelector.SelectedUnits[i];
				var matrix = Matrix4x4.TRS(config.Position, Quaternion.Euler(config.Rotation), Vector3.one);
				matrix = unit.transform.localToWorldMatrix * matrix;
				Matrices[i] = matrix;
				Colors[i] = Renderer.Player.FactionID == unit.FactionID ? config.AllyColor : config.EnemyColor;
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
