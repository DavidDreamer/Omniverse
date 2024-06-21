using Omniverse;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;

namespace Dreambox.Rendering.URP
{
	public class Outline: MonoBehaviour
	{
		[Inject]
		public OutlinePass Pass { get; private set; }
		
		[Inject]
		public EntityDetector EntityDetector { get; private set; }
		
		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}
		
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (cam.cameraType is CameraType.Game or CameraType.SceneView)
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		private void LateUpdate()
		{
			Pass.Clear();

			foreach (EntityPresenter entityPresenter in EntityDetector.Entities)
			{
				var renderers = entityPresenter.GetComponentsInChildren<Renderer>();
				foreach (Renderer r in renderers)
				{
					Pass.AddRenderer(r);
				}
			}
		}
	}
}
