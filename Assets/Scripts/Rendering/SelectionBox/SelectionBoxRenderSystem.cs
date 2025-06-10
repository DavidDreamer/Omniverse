using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class SelectionBoxRenderSystem : SystemBase
	{
		private SelectionBoxRenderSettings Settings { get; set; }

		private SelectionBoxRendererPass Pass { get; set; }

		protected override void OnStartRunning()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;

			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.SelectionBox;

			Pass = new SelectionBoxRendererPass(Settings.Material)
			{
				renderPassEvent = Settings.RenderPassEvent
			};
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		protected override void OnUpdate()
		{
			var selection = SystemAPI.GetSingleton<Selection>();

			if (!selection.InProcess)
			{
				return;
			}

			float x, z, y, w;

			if (selection.StartPosition.x > selection.EndPosition.x)
			{
				x = selection.EndPosition.x;
				z = selection.StartPosition.x;
			}
			else
			{
				x = selection.StartPosition.x;
				z = selection.EndPosition.x;
			}

			if (selection.StartPosition.y > selection.EndPosition.y)
			{
				y = selection.EndPosition.y;
				w = selection.StartPosition.y;
			}
			else
			{
				y = selection.StartPosition.y;
				w = selection.EndPosition.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			Shader.SetGlobalVector(SelectionBoxShaderVariables.SelectionBox, selectionBox);
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			var selection = SystemAPI.GetSingleton<Selection>();

			if (!selection.InProcess)
			{
				return;
			}

			if (Settings.CameraType.HasFlag(cam.cameraType))
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}
	}
}
