using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using VContainer;

[Preserve]
public class AbilityController
{
	public void TryCastAbility(Ability ability)
	{
		AbilityCastError error = ability.CanBeCasted();

		if (error is not AbilityCastError.None)
		{
			//ControlPanel.ErrorMessage.Show(error);
			return;
		}

		ability.Cast(default).SuppressCancellationThrow().Forget();
	}
}
