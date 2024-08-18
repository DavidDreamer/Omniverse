using Omniverse.Input;
using System.Collections.Generic;
using System;
using UnityEngine;
using VContainer.Unity;
using VContainer;

namespace Omniverse.Rendering
{
	public class Navigator : IInitializable, ILateTickable, IDisposable
	{
		[Inject]
		public UnitController UnitController { get; private set; }

		[Inject]
		public NavigationRendererFeature NavigationRendererFeature { get; private set; }

		public void Initialize()
		{
			UnitController.NavigationPointCreated += OnNavigationPointCreated;
		}

		public void Dispose()
		{
			UnitController.NavigationPointCreated -= OnNavigationPointCreated;
		}

		public void LateTick()
		{
			Queue<NavigationPoint> points = NavigationRendererFeature.Points;

			float time = Time.time;

			while (points.Count > 0)
			{
				NavigationPoint navigationPoint = points.Peek();

				float lifetime = time - navigationPoint.Time;

				if (lifetime >= NavigationRendererFeature.Config.Lifetime)
				{
					points.Dequeue();
				}
				else
				{
					break;
				}
			}
		}

		private void OnNavigationPointCreated(Vector3 point)
		{
			Queue<NavigationPoint> points = NavigationRendererFeature.Points;

			var navigationPointData = new NavigationPoint
			{
				Position = point,
				Time = Time.time
			};

			if (points.Count == NavigationRendererFeature.Config.Capacity)
			{
				points.Dequeue();
			}

			points.Enqueue(navigationPointData);
		}
	}
}
