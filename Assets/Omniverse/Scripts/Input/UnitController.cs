using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
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
			if (!Mouse.current.rightButton.wasReleasedThisFrame)
			{
				return;
			}
			
			if (UnitSelector.SelectedUnits.Count == 0)
			{
				return;
			}

			Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.value);

			if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity))
			{
				var unitPresenter = hit.transform.GetComponentInParent<UnitPresenter>();

				if (unitPresenter != null)
				{
					foreach (Unit selectedUnit in UnitSelector.SelectedUnits)
					{
						selectedUnit.Target = unitPresenter.Unit;
					}

					return;
				}
			}
			
			ProcessNavigationPoint();
		}

		private void ProcessNavigationPoint()
		{
			if (NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 position))
			{
				NavigationPoint navigationPoint =
					Object.Instantiate(Config.NavigationPointPrefab, position, Quaternion.identity);

				AnimatieNavigationPoint(navigationPoint, navigationPoint.destroyCancellationToken).Forget();

				foreach (Unit unit in UnitSelector.SelectedUnits)
				{
					unit.Target = null;
					unit.Presenter.NavMeshAgent.destination = position;
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
