using Omniverse.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Installer")]
	public class OmniverseInstaller: ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private PhysicsSettings PhysicsSettings { get; set; }
		
		[field: SerializeField]
		private UnitManagerConfig UnitManagerConfig { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(PhysicsSettings);
			builder.RegisterInstance(UnitManagerConfig);

			builder.Register<Player>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
		}
	}
}
