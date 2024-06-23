using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using UnityEngine.Scripting;

[Preserve]
public abstract class TargetAbilityController: AbilityController, IDisposable
{
	protected Ability Ability { get; private set; }
	
	private CancellationTokenSource CancellationTokenSource { get; set; }

	public bool InProcess => Ability is not null;

	protected virtual void Setup(Ability ability)
	{
		Ability = ability;
		ability.AwaitsTarget = true;

		CancellationTokenSource = new CancellationTokenSource();
		CancellationTokenSource.Token.Register(Cleanup);
	}
	
	protected virtual void Cleanup()
	{
		Ability.AwaitsTarget = false;
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
