using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Installer")]
	public class OmniverseInstaller : ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private UnitManagerConfig UnitManagerConfig { get; set; }

		public void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(UnitManagerConfig);
		}
	}
}
