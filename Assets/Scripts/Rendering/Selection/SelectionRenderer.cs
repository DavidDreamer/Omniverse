using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class SelectionRenderer : MonoBehaviour, IInitializable
	{
		[Inject]
		public SelectionConfig Config { get; private set; }

		[Inject]
		public UnitSelector UnitSelector { get; private set; }

		[Inject]
		public Player Player { get; private set; }

		private SelectionPass Pass { get; set; }

		public void Initialize()
		{
			Pass = new SelectionPass(this)
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
