using Dreambox.Core;
using Omniverse.Abilities;
using Omniverse.Input;
using Unity.Entities;
using Unity.Transforms;
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

			EntityManager entityManager = ECSUtils.ClientWorld.EntityManager;
			var abilityInput = ECSUtils.GetSingletonManaged<AbilityInput>();

			var transform = entityManager.GetComponentData<LocalTransform>(abilityInput.Entity);
			var localToWorld = entityManager.GetComponentData<LocalToWorld>(abilityInput.Entity);
			var abilityModule = entityManager.GetComponentObject<AbilityModule>(abilityInput.Entity);
			var ability = abilityInput.Ability;

			DrawRange();
			DrawDireciton();

			void DrawRange()
			{
				AbilityRendererConfig config = Renderer.Config;

				var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Scale(Vector3.one * ability.CastRange * 2f) * MatrixUtils.WorldUpRotation;

				var drawMeshParams = config.Range;
				commandBuffer.DrawMesh(
					drawMeshParams.Mesh,
					matrix,
					drawMeshParams.Material,
					drawMeshParams.SubmeshIndex,
					drawMeshParams.ShaderPass);
			}

			void DrawDireciton()
			{
				if (ability.Target is not VectorTarget vectorTarget || vectorTarget.Mode is not VectorTargetMode.Direction)
				{
					return;
				}

				AbilityDirectionRendererData config = Renderer.Config.Direction;

				var pointer = ECSUtils.GetSingleton<Pointer>();

				if (pointer.TargetType is not PointerTargetType.World)
				{
					return;
				}

				Vector3 activeUnitPosition = transform.Position;
				Vector3 direction = (Vector3)pointer.WorldPosition - activeUnitPosition;
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
}
