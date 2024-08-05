using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Units;
using VContainer;

namespace Omniverse.Input
{
	public class EntityTargetHandler : TargetAbilityHandler
	{
		[Inject]
		private EntityDetector EntityDetector { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		protected override async UniTask GetTarget(Unit caster, CancellationToken token)
		{
			TargetDesc abiltiyTargetDesc= Ability.Desc.Target;

			EntityDetector.ClearFilter();

			if (abiltiyTargetDesc.Type.HasFlag(TargetType.ResourceSource))
			{
				EntityDetector.AddToFilter<ResourceSource>();
			}

			if (abiltiyTargetDesc.Type.HasFlag(TargetType.Unit))
			{
				EntityDetector.AddToFilter<Unit>();
			}

			bool inputProcessed;
			bool hasTarget;

			do
			{
				await UniTask.NextFrame(token);

				inputProcessed = CommonActions.Apply.WasPressedThisFrame();
				hasTarget = EntityDetector.Target != null;
			}
			while (!inputProcessed || !hasTarget);

			Ability.ExecutionContext.Entities.Add(EntityDetector.Target);

			EntityDetector.SetDefaultDetectableType();
		}
	}
}
