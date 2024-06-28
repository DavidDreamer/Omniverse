using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Entities.Units;
using VContainer;

namespace Omniverse.Input
{
	public class UnitTargetHandler: TargetAbilityHandler
	{
		[Inject]
		private EntityDetector EntityDetector { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		protected override async UniTask GetTarget(Unit caster, CancellationToken token)
		{
			EntityDetector.SetDetectableType<ResourceSource>();

			bool inputProcessed;
			bool hasTarget;

			do
			{
				await UniTask.NextFrame(token);

				inputProcessed = CommonActions.Apply.WasPressedThisFrame();
				hasTarget = EntityDetector.Target != null;
			}
			while (!inputProcessed || !hasTarget);

			EntityDetector.SetDefaultDetectableType();
		}
	}
}
