using Omniverse.Input;
using Omniverse.Entities.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Omniverse.Rendering;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Installer")]
	public class OmniverseInstaller: ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private PhysicsSettings PhysicsSettings { get; set; }
		
		[field: SerializeField]
		private UnitManagerConfig UnitManagerConfig { get; set; }
		
		[field: SerializeField]
		private InputInstaller InputInstaller { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(PhysicsSettings);
			builder.RegisterInstance(UnitManagerConfig);

			builder.Register<Player>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
			InputInstaller.Install(builder);
		}
	}
}
