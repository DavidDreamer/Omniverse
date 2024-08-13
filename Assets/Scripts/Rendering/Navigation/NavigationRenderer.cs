using System;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using VContainer;
using VContainer.Unity;
using System.Collections.Generic;

namespace Omniverse.Rendering
{
	public class NavigationRenderer : MonoBehaviour, IInitializable, ITickable, IDisposable
	{
		[Inject]
		public NavigationRenderConfig Config { get; private set; }

		[Inject]
		public UnitController UnitController { get; private set; }

		private NavigationRenderPass Pass { get; set; }

		public Queue<NavigationPoint> NavigationPoints { get; } = new();

		public void Initialize()
		{
			Pass = new NavigationRenderPass(this)
			{
				renderPassEvent = Config.RenderPassEvent
			};

			UnitController.NavigationPointCreated += OnNavigationPointCreated;
		}

		public void Dispose()
		{
			UnitController.NavigationPointCreated -= OnNavigationPointCreated;
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

		public void Tick()
		{
			float time = Time.time;

			while (NavigationPoints.Count > 0)
			{
				NavigationPoint navigationPoint = NavigationPoints.Peek();

				float lifetime = time - navigationPoint.Time;

				if (lifetime >= Config.Lifetime)
				{
					NavigationPoints.Dequeue();
				}
				else
				{
					break;
				}
			}
		}

		private void OnNavigationPointCreated(Vector3 point)
		{
			var navigationPointData = new NavigationPoint
			{
				Position = point,
				Time = Time.time
			};

			if (NavigationPoints.Count == Config.Capacity)
			{
				NavigationPoints.Dequeue();
			}

			NavigationPoints.Enqueue(navigationPointData);
		}
	}
}
