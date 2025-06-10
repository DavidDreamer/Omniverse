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
	public class AbilityRenderPass : ScriptableRenderPass
	{
		public Selection Selection { get; set; }

		public Pointer Pointer{ get; set; }

		private AbilityRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private class PassData
		{
		}

		public AbilityRenderPass(AbilityRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;
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

			var transform = EntityManager.GetComponentData<LocalTransform>(Selection.Entity);
			var localToWorld = EntityManager.GetComponentData<LocalToWorld>(Selection.Entity);
			var ability = Selection.Ability;

			DrawRange();
			DrawDireciton();

			void DrawRange()
			{
				if (!EntityManager.HasComponent<CastRange>(ability))
				{
					return;
				}

				var castRange = EntityManager.GetComponentData<CastRange>(ability);

				var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Scale(Vector3.one * castRange.Value * 2f) * MatrixUtils.WorldUpRotation;

				var settings = Settings.Range;
				commandBuffer.DrawMesh(
					settings.Mesh,
					matrix,
					settings.Material,
					settings.SubmeshIndex,
					settings.ShaderPass);
			}

			void DrawDireciton()
			{
				if (!EntityManager.HasComponent<AbilityTarget>(ability))
				{
					return;
				}

				var abilityTarget = EntityManager.GetComponentObject<AbilityTarget>(ability);

				if (abilityTarget.Target is not VectorTarget vectorTarget || vectorTarget.Mode is not VectorTargetMode.Direction)
				{
					return;
				}

				AbilityDirectionRendererData data = Settings.Direction;

				if (Pointer.TargetType is not PointerTargetType.World)
				{
					return;
				}

				Vector3 activeUnitPosition = transform.Position;
				Vector3 direction = (Vector3)Pointer.WorldPosition - activeUnitPosition;
				direction.Set(direction.x, 0, direction.z);
				direction.Normalize();

				Vector3 position = activeUnitPosition + direction * data.Scale.y * 0.5f;
				Quaternion rotation = Quaternion.LookRotation(Vector3.down, direction);
				Vector3 scale = data.Scale;

				var matrix = Matrix4x4.TRS(position, rotation, scale);

				var settings = data.MeshDrawSettings;
				commandBuffer.DrawMesh(
					settings.Mesh,
					matrix,
					settings.Material,
					settings.SubmeshIndex,
					settings.ShaderPass);
			}
		}
	}
}
