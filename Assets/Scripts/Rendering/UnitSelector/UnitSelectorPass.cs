using Dreambox.Rendering.Core;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class UnitSelectorPass : ScriptableRenderPass
	{
		private UnitSelectorRenderer UnitSelectorRenderer { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] Colors { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public UnitSelectorPass(UnitSelectorRenderer unitSelectorRenderer)
		{
			UnitSelectorRenderer = unitSelectorRenderer;

			Matrices = new Matrix4x4[UnitSelector.Capacity];
			Colors = new Vector4[UnitSelector.Capacity];
			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, nameof(UnitSelectorPass));
			var commandBuffer = scope.CommandBuffer;

			UnitSelectorRenderingConfig config = UnitSelectorRenderer.Config;
			UnitSelector unitSelector = UnitSelectorRenderer.UnitSelector;

			MaterialPropertyBlock.Clear();

			for (int i = 0; i < unitSelector.SelectedUnits.Count; i++)
			{
				Entities.Units.Unit unit = unitSelector.SelectedUnits[i];
				var matrix = Matrix4x4.TRS(config.Position, Quaternion.Euler(config.Rotation), Vector3.one);
				matrix = unit.transform.localToWorldMatrix * matrix;
				Matrices[i] = matrix;
				Colors[i] = UnitSelectorRenderer.Player.FactionID == unit.FactionID ? config.AllyColor : config.EnemyColor;
			}

			MaterialPropertyBlock.SetVectorArray("_Color", Colors);

			commandBuffer.DrawMeshInstanced(
				config.Mesh,
				0,
				config.Material,
				config.ShaderPass,
				Matrices,
				unitSelector.SelectedUnits.Count,
				MaterialPropertyBlock);
		}
	}
}
