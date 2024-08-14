using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class HealthBarRenderer : MonoBehaviour, IInitializable
	{
		[Inject]
		public HealthBarRenderConfig Config { get; private set; }

		[Inject]
		public Units.Manager UnitManager { get; private set; }

		[Inject]
		public Player Player { get; private set; }

		private HealthBarRenderPass Pass { get; set; }

		public void Initialize()
		{
			Pass = new HealthBarRenderPass(this)
			{
				renderPassEvent = Config.RenderPassEvent
			};
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
