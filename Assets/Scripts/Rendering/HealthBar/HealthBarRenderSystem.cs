using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class HealthBarRenderSystem : SystemBase
	{
		private HealthBarRenderSettings Settings { get; set; }

		private HealthBarRenderPass Pass { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<Player>();
		}

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.HealthBar;

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
			if (!SystemAPI.HasSingleton<Player>())
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
			Pass.Player = SystemAPI.GetSingleton<Player>();
		}
	}
}
