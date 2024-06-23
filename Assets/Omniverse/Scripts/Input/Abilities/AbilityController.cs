using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using VContainer;

[Preserve]
public class AbilityController
{
	public void TryCastAbility(Unit unit, Ability ability)
	{
		AbilityCastError error = ability.CanBeCasted(unit);

		if (error is not AbilityCastError.None)
		{
			//ControlPanel.ErrorMessage.Show(error);
			return;
		}

		ability.Cast(unit, default).SuppressCancellationThrow().Forget();
	}
}
