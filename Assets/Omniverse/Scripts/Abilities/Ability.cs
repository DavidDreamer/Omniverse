using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using ExecutionContext = Omniverse.Actions.ExecutionContext;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public ITarget Target { get; }
		
		public Cooldown Cooldown { get; }
		
		public bool AwaitsTarget { get; set; }

		public bool InProcess { get; private set; }
		
		public AutoResetUniTaskCompletionSource Used { get; }

		private ExecutionContext ExecutionContext { get; }
		
		public Ability(IObjectResolver objectResolver, AbilityDesc desc)
		{
			Desc = desc;
			Target = desc.Target.Build();
			Cooldown = new Cooldown(Desc.Cooldown);
			Used = AutoResetUniTaskCompletionSource.Create();
			ExecutionContext = new ExecutionContext(objectResolver, desc.Actions);
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
			
			switch (Target)
			{
				case null:
					break;
				case EntityTarget entityTarget:
					ExecutionContext.Entities.Add(entityTarget.Value);
					break;
				case PointTarget pointTarget:
					ExecutionContext.Points.Add(pointTarget.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Target));
			}
			
			await ExecutionContext.PerformAsync(caster, token);

			Used.TrySetResult();
		}
	}
}
