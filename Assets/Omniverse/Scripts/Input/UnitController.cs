using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units.Rendering;
using UnityEngine;
using UnityEngine.AI;
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
				var unitRenderer = hit.transform.GetComponentInChildren<UnitRenderer>();

				if (unitRenderer != null)
				{
					foreach (UnitRenderer selectedUnit in UnitSelector.SelectedUnits)
					{
						if (selectedUnit.Unit != unitRenderer.Unit)
						{
							selectedUnit.Unit.Target = unitRenderer.Unit;
						}
					}

					return;
				}
			}
			
			ProcessNavigationPoint();
		}

		public void ProcessNavigationPoint(Vector3 position)
		{
			if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, float.MaxValue, 1))
			{
				CreateNavigationPoint(navMeshHit.position);
			}
		}
		
		private void ProcessNavigationPoint()
		{
			if (NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 position))
			{
				CreateNavigationPoint(position);
			}
		}

		private void CreateNavigationPoint(Vector3 position)
		{
			NavigationPoint navigationPoint =
				Object.Instantiate(Config.NavigationPointPrefab, position, Quaternion.identity);

			AnimatieNavigationPoint(navigationPoint, navigationPoint.destroyCancellationToken).Forget();

			foreach (UnitRenderer unitRenderer in UnitSelector.SelectedUnits)
			{
				unitRenderer.Unit.Target = null;
				unitRenderer.Unit.Presenter.NavMeshAgent.destination = position;
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
