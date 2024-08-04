using Dreambox.Rendering.Core;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class HealthBarPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));

			public static int SecondColor { get; } = Shader.PropertyToID(nameof(SecondColor));

			public static int Amount { get; } = Shader.PropertyToID(nameof(Amount));
		}

		private HealthBarRenderer Renderer { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] BaseColors { get; }

		private Vector4[] SecondColors { get; }

		private float[] Amounts { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public HealthBarPass(HealthBarRenderer renderer)
		{
			Renderer = renderer;

			Matrices = new Matrix4x4[renderer.Config.MaxCount];
			BaseColors = new Vector4[renderer.Config.MaxCount];
			SecondColors = new Vector4[renderer.Config.MaxCount];
			Amounts = new float[renderer.Config.MaxCount];

			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "Health Bars");
			var commandBuffer = scope.CommandBuffer;

			HealthBarConfig config = Renderer.Config;
			Units.Manager unitManager = Renderer.UnitManager;
			var units = unitManager.Units;

			MaterialPropertyBlock.Clear();

			int count = Mathf.Min(units.Count, config.MaxCount);

			if (count == 0)
			{
				return;
			}

			for (int i = 0; i < count; i++)
			{
				Unit unit = units[i];

				var matrix = unit.transform.localToWorldMatrix * Matrix4x4.Translate(config.Offset);
				Matrices[i] = matrix;

				HealthBarColors colors = Renderer.Player.FactionID == unit.FactionID ? config.AllyColors : config.EnemyColors;
				BaseColors[i] = colors.BaseColor;
				SecondColors[i] = colors.SecondColor;

				var healthProperty = unit.Properties[PropertyID.Health];
				Amounts[i] = healthProperty.Amount.Value / healthProperty.Desc.Range.Max;
			}

			MaterialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColors);
			MaterialPropertyBlock.SetVectorArray(ShaderVariables.SecondColor, SecondColors);
			MaterialPropertyBlock.SetFloatArray(ShaderVariables.Amount, Amounts);

			var drawMeshParams = config.DrawMeshParams;
			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				count,
				MaterialPropertyBlock);
		}
	}
}
