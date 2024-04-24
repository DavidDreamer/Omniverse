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
		private UnitPresenter UnitPresenter { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(PhysicsSettings);
			builder.RegisterInstance(UnitPresenter);
			
			builder.Register<PrefabPool<UnitPresenter>>(Lifetime.Singleton);
			builder.Register<PrefabPool<ItemPresenter>>(Lifetime.Singleton);
			
			builder.RegisterEntryPoint<ItemManager>().AsSelf();
			builder.RegisterEntryPoint<UnitManager>().AsSelf();
		}
	}
}
