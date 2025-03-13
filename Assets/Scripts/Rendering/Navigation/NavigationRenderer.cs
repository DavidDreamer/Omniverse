using System.Collections.Generic;
using Dreambox.Core;
using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class NavigationRenderer : CustomRenderer<NavigationRendererConfig, NavigationRenderPass>
	{
		private Queue<NavigationPoint> Points { get; } = new();

		protected override NavigationRenderPass Setup() => new(Config, Points);

		protected override bool IsInactive() => Points.Count == 0;

		public void Awake()
		{
			ProcessCommandInputSystem.NavigationPointCreated += OnNavigationPointCreated;
		}

		public void OnDestroy()
		{
			ProcessCommandInputSystem.NavigationPointCreated -= OnNavigationPointCreated;
		}

		private void LateUpdate()
		{
			float time = Time.time;

			while (Points.Count > 0)
			{
				NavigationPoint point = Points.Peek();

				float lifetime = time - point.Time;

				if (lifetime >= Config.Lifetime)
				{
					Points.Dequeue();
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
				Matrix = Matrix4x4.TRS(point, Quaternion.identity, Vector3.one) * MatrixUtils.WorldUpRotation,
				Time = Time.time
			};

			if (Points.Count == Config.Capacity)
			{
				Points.Dequeue();
			}

			Points.Enqueue(navigationPointData);
		}
	}
}
