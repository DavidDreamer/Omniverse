using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Items;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Omniverse.Input
{
	public class UnitController : ITickable
	{
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		[Inject]
		private EntityDetector EntityDetector { get; set; }

		private UnitControllerConfig Config { get; }

		public UnitController(UnitControllerConfig config)
		{
			Config = config;
		}

		public void Tick()
		{
			if (UnitSelector.SelectedUnits.Count == 0)
			{
				return;
			}

			if (Mouse.current.rightButton.wasReleasedThisFrame)
			{
				if (EntityDetector.Target != null)
				{
					Entity target = EntityDetector.Target;
					switch (target)
					{
						case Unit unit:
							foreach (Unit selectedUnit in UnitSelector.SelectedUnits)
							{
								if (selectedUnit != unit)
								{
									selectedUnit.Target = unit;
								}
							}

							break;
						case Item item:
							foreach (Unit selectedUnit in UnitSelector.SelectedUnits)
							{
								selectedUnit.Target = item;
							}

							break;
					}
				}
				else
				{
					ProcessNavigationPoint();
				}
			}
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

			foreach (Unit unit in UnitSelector.SelectedUnits)
			{
				unit.MoveToPosition(position);
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
