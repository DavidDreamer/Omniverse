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
			
			builder.Register<Player>(Lifetime.Singleton).AsSelf();

			builder.Register<PrefabPool<UnitPresenter>>(Lifetime.Singleton);
			builder.Register<PrefabPool<UnitRenderer>>(Lifetime.Singleton);
			builder.Register<PrefabPool<ItemPresenter>>(Lifetime.Singleton);
			
			builder.RegisterEntryPoint<ItemManager>().AsSelf();
			builder.RegisterEntryPoint<UnitManager>().AsSelf();
		}
	}
}
