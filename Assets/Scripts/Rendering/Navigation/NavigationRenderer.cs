using System.Collections.Generic;
using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class NavigationRenderer : CustomRenderer<NavigationRendererConfig, NavigationRenderPass>, ILateTickable
	{
		[Inject]
		public UnitController UnitController { get; private set; }

		public Queue<NavigationPoint> Points { get; } = new();

		protected override NavigationRenderPass CreatePass() => new(this);

		protected override bool IsInactive() => Points.Count == 0;

		public override void Initialize()
		{
			base.Initialize();

			UnitController.NavigationPointCreated += OnNavigationPointCreated;
		}

		public override void Dispose()
		{
			base.Dispose();

			UnitController.NavigationPointCreated -= OnNavigationPointCreated;
		}

		public void LateTick()
		{
			float time = Time.time;

			while (Points.Count > 0)
			{
				NavigationPoint navigationPoint = Points.Peek();

				float lifetime = time - navigationPoint.Time;

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
				Position = point,
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
