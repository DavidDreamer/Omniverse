using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Units;
using UnityEditor.Playables;
using VContainer;

namespace Omniverse.Input
{
	public abstract class TargetAbilityHandler : AbilityHandler, IDisposable
	{
		protected Ability Ability { get; private set; }

		private CancellationTokenSource CancellationTokenSource { get; set; }

		public bool InProcess => Ability is not null;

		[Inject]
		private UnitController UnitController { get; set; }

		protected virtual void Setup(Ability ability)
		{
			Ability = ability;
			UnitController.ActiveAbility = ability;

			CancellationTokenSource = new CancellationTokenSource();
			CancellationTokenSource.Token.Register(Cleanup);
		}

		protected virtual void Cleanup()
		{
			UnitController.ActiveAbility = null;
			Ability = null;

			CancellationTokenSource.Dispose();
			CancellationTokenSource = null;
		}

		public void Dispose()
		{
			CancellationTokenSource?.Dispose();
		}

		public async UniTask GetTargetAndCast(Unit caster, Ability ability)
		{
			AbilityCastError error = ability.CanBeCasted(caster);

			if (error is not AbilityCastError.None)
			{
				//ControlPanel.ErrorMessage.Show(error);
				return;
			}

			Setup(ability);

			await GetTarget(caster, CancellationTokenSource.Token);

			Cleanup();

			TryCastAbility(caster, ability);
		}

		public void Cancell() => CancellationTokenSource.Cancel();

		protected abstract UniTask GetTarget(Unit caster, CancellationToken token);
	}
}
