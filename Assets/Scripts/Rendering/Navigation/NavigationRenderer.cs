using System.Collections.Generic;
using Dreambox.Core;
using Dreambox.Rendering.Universal;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class NavigationRenderer : CustomRenderer<NavigationRendererConfig, NavigationRenderPass>, ILateTickable
	{
		private Queue<NavigationPoint> Points { get; } = new();

		[Inject]
		public UnitController UnitController { get; private set; }

		protected override NavigationRenderPass CreatePass() => new(Config, Points);

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
