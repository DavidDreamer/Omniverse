using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
    [CreateAssetMenu]
	public class OmniverseInstaller: ScriptableObject, IInstaller
	{
		public void Install(IContainerBuilder builder)
		{
			builder.Register<PrefabPool<UnitPresenter>>(Lifetime.Singleton);
			builder.Register<PrefabPool<ItemPresenter>>(Lifetime.Singleton);
			
			builder.RegisterEntryPoint<FactionManager>().AsSelf().WithParameter(GlobalSettings.Instance.Factions.ToList());
			builder.RegisterEntryPoint<ItemManager>().AsSelf();
			builder.RegisterEntryPoint<UnitManager>().AsSelf();
		}
	}
}
