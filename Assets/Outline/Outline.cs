using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Dreambox.Rendering.URP
{
	public class Outline: MonoBehaviour
	{
		public Renderer[] renderers;

		private void OnValidate()
		{
			if (Pass != null)
			{
				Pass.Clear();
				foreach (Renderer r in renderers)
				{
					Pass.AddRenderer(r);
				}
			}
		}

		[field: SerializeField]
		public Shader Shader { get; private set; }

		[field: SerializeField] private OutlineConfig[] OutlineConfigs { get; set; }

		private OutlinePass Pass { get; set; }

		public void Awake()
		{
			Pass = new OutlinePass(Shader, OutlineConfigs)
			{
				renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
			};
		}

		public void OnDestroy()
		{
			Pass.Dispose();
		}

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
	}
}
