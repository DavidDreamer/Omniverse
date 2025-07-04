using System.Collections.Generic;
using Dreambox.Core;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class NavigationRenderSystem : SystemBase
	{
		private Queue<NavigationPoint> Points { get; } = new();

		private NavigationRenderSettings Settings { get; set; }

		private NavigationRenderPass Pass { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Navigation;

			Pass = new(Settings, Points)
			{
				renderPassEvent = Settings.RenderPassEvent
			};

			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
			ProcessCommandInputSystem.NavigationPointCreated += OnNavigationPointCreated;
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
			ProcessCommandInputSystem.NavigationPointCreated -= OnNavigationPointCreated;
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (Points.Count == 0)
			{
				return;
			}

			if (Settings.CameraType.HasFlag(camera.cameraType))
			{
				camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		private void OnNavigationPointCreated(Vector3 point)
		{
			var navigationPointData = new NavigationPoint
			{
				Matrix = Matrix4x4.TRS(point, Quaternion.identity, Vector3.one) * MatrixUtils.WorldUpRotation,
				Time = SystemAPI.Time.ElapsedTime
			};

			if (Points.Count == Settings.Capacity)
			{
				Points.Dequeue();
			}

			Points.Enqueue(navigationPointData);
		}

		protected override void OnUpdate()
		{
			double time = SystemAPI.Time.ElapsedTime;

			while (Points.Count > 0)
			{
				NavigationPoint point = Points.Peek();

				double lifetime = time - point.Time;

				if (lifetime >= Settings.Lifetime)
				{
					Points.Dequeue();
				}
				else
				{
					break;
				}
			}
		}
	}
}
