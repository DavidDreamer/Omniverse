using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Camera
{
	public class UnitController: ITickable
	{
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		private UnitControllerConfig Config { get; }

		public UnitController(UnitControllerConfig config)
		{
			Config = config;
		}

		public void Tick()
		{
			if (Mouse.current.rightButton.wasReleasedThisFrame)
			{
				ProcessNavigationPoint();
			}
		}

		private void ProcessNavigationPoint()
		{
			if (UnitSelector.SelectedUnits.Count == 0)
			{
				return;
			}
			
			if (NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 position))
			{
				NavigationPoint navigationPoint =
					Object.Instantiate(Config.NavigationPointPrefab, position, Quaternion.identity);

				AnimatieNavigationPoint(navigationPoint, navigationPoint.destroyCancellationToken).Forget();

				foreach (var pair in UnitSelector.SelectedUnits)
				{
					pair.Key.Presenter.NavMeshAgent.destination = position;
				}
			}
		}
		
		private async UniTaskVoid AnimatieNavigationPoint(NavigationPoint navigationPoint, CancellationToken token)
		{
			float duration = Config.NavigationPointDuration;
			float time = 0;

			navigationPoint.SetPower(0);

			while (time < duration)
			{
				await UniTask.NextFrame(token);
				time = Mathf.Min(time + Time.deltaTime, duration);
				navigationPoint.SetPower(time / duration);
			}

			Object.Destroy(navigationPoint.gameObject);
		}
	}
}
