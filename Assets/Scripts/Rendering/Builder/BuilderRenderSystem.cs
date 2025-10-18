using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class BuilderRenderSystem : SystemBase
	{
		private BuilderRenderSettings Settings { get; set; }

		private BuilderRenderPass Pass { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Builder;

			Pass = new(Settings, EntityManager)
			{
				renderPassEvent = Settings.RenderPassEvent
			};

			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			var builder = SystemAPI.GetSingleton<Builder>();

			if (!builder.InProcess)
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
			var builder = SystemAPI.GetSingleton<Builder>();
			var pointer = SystemAPI.GetSingleton<Pointer>();

			Pass.Building = builder.Building;
			Pass.Pointer = pointer;
		}
	}
}
