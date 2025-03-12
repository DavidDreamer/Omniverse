using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Installer")]
	public class OmniverseInstaller : ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private PhysicsSettings PhysicsSettings { get; set; }

		[field: SerializeField]
		private UnitManagerConfig UnitManagerConfig { get; set; }

		[field: SerializeField]
		private InputInstaller InputInstaller { get; set; }

		public void Install(IContainerBuilder builder)
		{
			builder.Register<PhysicsService>(Lifetime.Singleton).WithParameter(PhysicsSettings);
			builder.RegisterInstance(UnitManagerConfig);

			InputInstaller.Install(builder);
		}
	}
}
