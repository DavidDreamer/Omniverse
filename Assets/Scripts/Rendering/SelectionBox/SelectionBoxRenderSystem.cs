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
		private SelectionBoxRendererConfig Config { get; set; }

		private SelectionBoxRendererPass Pass { get; set; }

		protected override void OnCreate()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;

			var rendering = Object.FindFirstObjectByType<RenderingClient>(FindObjectsInactive.Include);
			Config = rendering.SelectionBoxRendererConfig;
			Pass = new SelectionBoxRendererPass(Config.Material);
		}

		protected override void OnDestroy()
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

			if (Config.CameraType.HasFlag(cam.cameraType))
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}
	}
}
