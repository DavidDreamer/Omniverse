using Omniverse.Items;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<PrefabPool<Item>>(Lifetime.Singleton);

			builder.RegisterEntryPoint<ItemManager>().AsSelf();
			builder.RegisterEntryPoint<TempNameManager>().AsSelf();

			builder.Register<GameDirector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		}
	}
}
