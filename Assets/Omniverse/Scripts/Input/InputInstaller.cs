using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Input")]
	public class InputInstaller: ScriptableObject, IInstaller
	{
		public void Install(IContainerBuilder builder)
		{
			var inputActions = new InputActions();
			builder.RegisterInstance(inputActions.Abilities);
		}
	}
}
