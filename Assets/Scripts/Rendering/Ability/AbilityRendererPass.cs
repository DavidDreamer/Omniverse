using Dreambox.Core;
using Omniverse.Abilities;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class AbilityRendererPass : ScriptableRenderPass
	{
		private AbilityRenderer Renderer { get; }

		private class PassData
		{
		}

		public AbilityRendererPass(AbilityRenderer renderer)
		{
			Renderer = renderer;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Ability", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;

			DrawRange(commandBuffer);
			DrawDireciton(commandBuffer);
		}

		public void DrawRange(RasterCommandBuffer commandBuffer)
		{
			AbilityRendererConfig config = Renderer.Config;
			AbilityController abilityController = Renderer.AbilityController;

			var matrix = abilityController.ActiveUnit.transform.localToWorldMatrix *
				Matrix4x4.Scale(Vector3.one * abilityController.ActiveAbility.Desc.Casting.Range * 2f) *
				MatrixUtils.WorldUpRotation;

			var drawMeshParams = config.Range;
			commandBuffer.DrawMesh(
				drawMeshParams.Mesh,
				matrix,
				drawMeshParams.Material,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.ShaderPass);
		}

		private void DrawDireciton(RasterCommandBuffer commandBuffer)
		{
			var target = Renderer.AbilityController.ActiveAbility.Desc.Target;

			if (target is not VectorTarget vectorTarget || vectorTarget.Mode is not VectorTargetMode.Direction)
			{
				return;
			}

			AbilityDirectionRendererData config = Renderer.Config.Direction;
			InputController inputController = Renderer.InputController;
			AbilityController abilityController = Renderer.AbilityController;

			if (!inputController.CursorWorldPosition.HasValue)
			{
				return;
			}

			Vector3 activeUnitPosition = abilityController.ActiveUnit.transform.position;
			Vector3 direction = inputController.CursorWorldPosition.Value - activeUnitPosition;
			direction.Set(direction.x, 0, direction.z);
			direction.Normalize();

			Vector3 position = activeUnitPosition + direction * config.Scale.y * 0.5f;
			Quaternion rotation = Quaternion.LookRotation(Vector3.down, direction);
			Vector3 scale = config.Scale;

			var matrix = Matrix4x4.TRS(position, rotation, scale);

			var drawMeshParams = config.DrawMeshParams;
			commandBuffer.DrawMesh(
				drawMeshParams.Mesh,
				matrix,
				drawMeshParams.Material,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.ShaderPass);
		}
	}
}
