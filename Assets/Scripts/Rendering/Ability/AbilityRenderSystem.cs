using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class AbilityRenderSystem : SystemBase
	{
		private AbilityRenderSettings Settings { get; set; }

		private AbilityRenderPass Pass { get; set; }

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Ability;

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
			var selection = SystemAPI.GetSingleton<Selection>();

			if (!selection.AbilityInProcess)
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
			Pass.Selection = SystemAPI.GetSingleton<Selection>();
			Pass.Pointer = SystemAPI.GetSingleton<Pointer>();
		}
	}
}
