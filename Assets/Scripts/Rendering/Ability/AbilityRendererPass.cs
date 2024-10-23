using System;
using Dreambox.Core;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class AbilityRendererPass : ScriptableRenderPass, IDisposable
	{
		private AbilityRenderer Renderer { get; }

		public AbilityRendererPass(AbilityRenderer renderer)
		{
			Renderer = renderer;
		}

		public void Dispose()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "Ability");
			var commandBuffer = scope.CommandBuffer;

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

			DrawDireciton(commandBuffer);
		}

		private void DrawDireciton(CommandBuffer commandBuffer)
		{
			var target = Renderer.AbilityController.ActiveAbility.Desc.Target;
			if (!target.Type.HasFlag(Abilities.TargetType.Direction))
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
