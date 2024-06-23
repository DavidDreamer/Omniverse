using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

[Preserve]
public class UnitTargetController: TargetAbilityController
{
	[Inject]
	private InputActions.CommonActions CommonActions { get; set; }

	protected override async UniTask GetTarget(CancellationToken token)
	{
		while (true)
		{
			Vector2 mousePosition = Mouse.current.position.value;

			Ray ray = Camera.main.ScreenPointToRay(mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Hitbox")))
			{
				var unitPresenter = hit.collider.GetComponentInParent<UnitPresenter>();

				if (unitPresenter != null)
				{
					if (CommonActions.Apply.WasPressedThisFrame())
					{
						//TODO
						//Ability.Context.Units.Clear();
						//Ability.Context.Units.Add(unitPresenter.Entity);
						return;
					}
				}
			}

			await UniTask.NextFrame(token);
		}
	}
}
