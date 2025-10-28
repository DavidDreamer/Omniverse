using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class GridRenderSystem : SystemBase
	{
		private GridRenderSettings Settings { get; set; }

		private GridRenderPass Pass { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Grid;

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
			if (Settings.CameraType.HasFlag(camera.cameraType))
			{
				camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		protected override void OnUpdate()
		{
		}
	}
}
