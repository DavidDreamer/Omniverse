using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class SelectionRenderSystem : SystemBase
	{
		private SelectionRenderSettings Settings { get; set; }

		private SelectionRenderPass Pass { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<Player>();
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;

			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Selection;

			Pass = new SelectionRenderPass(Settings, EntityManager)
			{
				renderPassEvent = Settings.RenderPassEvent
			};
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (!SystemAPI.HasSingleton<Player>())
			{
				return;
			}

			if (Settings.CameraType.HasFlag(cam.cameraType))
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		protected override void OnUpdate()
		{
			Pass.Player = SystemAPI.GetSingleton<Player>();
			Pass.Selection = SystemAPI.GetSingleton<Selection>();
		}
	}
}
