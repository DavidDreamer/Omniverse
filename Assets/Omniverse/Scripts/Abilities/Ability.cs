using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ExecutionContext = Omniverse.Actions.ExecutionContext;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Cooldown Cooldown { get; }
		
		public bool AwaitsTarget { get; set; }

		public bool InProcess { get; private set; }
		
		public Ability(AbilityDesc desc)
		{
			Desc = desc;
		
			Cooldown = new Cooldown(Desc.Cooldown);
		}

		public AbilityCastError CanBeCasted(IEntity caster)
		{
			if (Cooldown is not null && Cooldown.IsActive)
			{
				return AbilityCastError.IsOnCooldown;
			}

			if (InProcess)
			{
				return AbilityCastError.AlreadyInProcess;
			}

			foreach (CostDesc costDesc in Desc.Cost)
			{
				if (!caster.Properties.ContainsKey(costDesc.PropertyID))
				{
					return AbilityCastError.NotEnoughResources;
				}

				if (caster.Properties[costDesc.PropertyID].Amount.Value < costDesc.Amount)
				{
					return AbilityCastError.NotEnoughResources;
				}
			}
			
			return AbilityCastError.None;
		}
		
		public async UniTask Cast(IEntity caster, CancellationToken token)
		{
			InProcess = true;

			if (!string.IsNullOrEmpty(Desc.Cast.AnimationTrigger))
			{
				//TODO:
				//Unit.Presenter.Animator.SetTrigger(AnimatorParameter.Get(Desc.Cast.AnimationTrigger));
			}

			await UniTask.Delay(TimeSpan.FromSeconds(Desc.Cast.Time), cancellationToken: token);
			
			foreach (CostDesc cost in Desc.Cost)
			{
				caster.Properties[cost.PropertyID].Change(-cost.Amount);
			}
			
			Cooldown?.ActivateAsync(token);

			InProcess = false;

			var contex = new ExecutionContext(caster, Desc.Actions);
			await contex.PerformAsync(token);
		}
	}
}
