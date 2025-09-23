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

		public Pointer Pointer { get; set; }

		private AbilityRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private class PassData
		{
			public EntityManager EntityManager;
			public Pointer Pointer;
			public AbilityRenderSettings AbilityRenderSettings;
			public Selection Selection;
		}

		public AbilityRenderPass(AbilityRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;

			ConfigureInput(ScriptableRenderPassInput.Depth);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Ability", out var data))
			{
				data.EntityManager = EntityManager;
				data.Pointer = Pointer;
				data.AbilityRenderSettings = Settings;
				data.Selection = Selection;

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0, AccessFlags.Write);
				builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.Read);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					var entityManager = data.EntityManager;
					var selection = data.Selection;
					var renderSettings = data.AbilityRenderSettings;
					var pointer = data.Pointer;

					var transform = entityManager.GetComponentData<LocalTransform>(selection.Entity);
					var localToWorld = entityManager.GetComponentData<LocalToWorld>(selection.Entity);
					var abilityBuffer = entityManager.GetBuffer<Ability>(selection.Entity);
					Ability ability = abilityBuffer[selection.AbilityIndex];

					DrawRange();
					DrawDireciton();

					void DrawRange()
					{
						if (ability.Casting.Range == 0)
						{
							return;
						}

						var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Scale(Vector3.one * ability.Casting.Range * 2f) * MatrixUtils.WorldUpRotation;

						var settings = renderSettings.Range;
						commandBuffer.DrawMesh(
							settings.Mesh,
							matrix,
							settings.Material,
							settings.SubmeshIndex,
							settings.ShaderPass);
					}

					void DrawDireciton()
					{
						if (ability.Desc.Value.Target is not VectorTarget vectorTarget || vectorTarget.Mode is not VectorTargetMode.Direction)
						{
							return;
						}

						AbilityDirectionRendererData data = renderSettings.Direction;

						if (pointer.TargetType is not PointerTargetType.World)
						{
							return;
						}

						Vector3 activeUnitPosition = transform.Position;
						Vector3 direction = (Vector3)pointer.WorldPosition - activeUnitPosition;
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
				});
			}
		}
	}
}
