using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using VContainer;

namespace Omniverse.Input
{
	public class AbilityHandler
	{
		[Inject]
		private ErrorHandler ErrorHandler { get; set; }
		
		public void TryCastAbility(Unit unit, Ability ability)
		{
			AbilityCastError error = ability.CanBeCasted(unit);

			if (error is not AbilityCastError.None)
			{
				ErrorHandler.Hadle(error.ToString());
				return;
			}

			ability.Cast(unit, default).SuppressCancellationThrow().Forget();
		}
	}
}
