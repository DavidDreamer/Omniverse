using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
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
	
	public async UniTask GetTargetAndCast(Ability ability)
	{
		AbilityCastError error = ability.CanBeCasted();

		if (error is not AbilityCastError.None)
		{
			//ControlPanel.ErrorMessage.Show(error);
			return;
		}
		
		Setup(ability);

		await GetTarget(CancellationTokenSource.Token);

		Cleanup();
		
		TryCastAbility(ability);
	}
	
	public void Cancell() => CancellationTokenSource.Cancel();

	protected abstract UniTask GetTarget(CancellationToken token);
}
