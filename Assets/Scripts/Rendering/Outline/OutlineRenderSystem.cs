using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dreambox.Rendering;
using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class OutlineRenderSystem : SystemBase
	{
		private OutlineRenderSettings Settings { get; set; }

		private OutlineRenderPass Pass { get; set; }

		private HashSet<OutlineRenderer> Targets { get; } = new();

		private ComputeBuffer VariantsBuffer { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<Player>();
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Outline;

			VariantsBuffer = new ComputeBuffer(Settings.Variants.Length, Marshal.SizeOf<OutlineVariant>());
			Settings.Material.SetBuffer(OutlineShaderVariable.VariantsBuffer, VariantsBuffer);

			float width = Settings.Variants.Max(config => config.Width);
			VariantsBuffer.SetData(Settings.Variants);

			Pass = new(Settings.Material, Targets, width)
			{
				renderPassEvent = Settings.RenderPassEvent
			};

			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		protected override void OnStopRunning()
		{
			VariantsBuffer.Release();

			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (!SystemAPI.HasSingleton<Player>())
			{
				return;
			}

			if (Targets.Count == 0)
			{
				return;
			}

			if (Settings.CameraType.HasFlag(camera.cameraType))
			{
				camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		protected override void OnUpdate()
		{
			Targets.Clear();

			var player = SystemAPI.GetSingleton<Player>();
			var pointer = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingleton<Selection>();

			var entity = pointer.Entity;

			if (entity == Entity.Null)
			{
				return;
			}

			if (selection.HasSelection && entity == selection.Entity)
			{
				return;
			}

			int outlineVariant = 0;
			if (SystemAPI.HasComponent<Faction>(entity))
			{
				var faction = SystemAPI.GetComponent<Faction>(entity);
				outlineVariant = faction.ID == player.FactionID ? 0 : 1;
			}
			else
			{
				outlineVariant = 2;
			}

			if (SystemAPI.HasComponent<MaterialMeshInfo>(entity))
			{
				AddTarget(entity);
			}

			if (SystemAPI.HasBuffer<Child>(entity))
			{
				var childBuffer = SystemAPI.GetBuffer<Child>(entity);

				for (int i = 0; i < childBuffer.Length; ++i)
				{
					Entity child = childBuffer[i].Value;

					if (!SystemAPI.HasComponent<MaterialMeshInfo>(child))
					{
						continue;
					}

					AddTarget(child);
				}
			}

			void AddTarget(Entity entity)
			{
				var localToWorld = SystemAPI.GetComponent<LocalToWorld>(entity);
				var materialMeshInfo = SystemAPI.GetComponent<MaterialMeshInfo>(entity);
				var renderMeshArray = EntityManager.GetSharedComponentManaged<RenderMeshArray>(entity);

				Mesh mesh = renderMeshArray.GetMesh(materialMeshInfo);
				Material material = renderMeshArray.GetMaterial(materialMeshInfo);
				Matrix4x4 matrix = localToWorld.Value;

				var outlineTarget = new OutlineRenderer(mesh, material, matrix, outlineVariant);
				Targets.Add(outlineTarget);
			}
		}
	}
}
