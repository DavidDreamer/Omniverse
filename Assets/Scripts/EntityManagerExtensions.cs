using Unity.Entities;

namespace Omniverse
{
	public static class EntityManagerExtensions
	{
		public static T GetSingleton<T>(this EntityManager entityManager) where T : unmanaged, IComponentData
		{
			var query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
			var singleton = query.GetSingleton<T>();
			query.Dispose();
			return singleton;
		}

		public static T GetSingletonManaged<T>(this EntityManager entityManager) where T : class, IComponentData
		{
			var query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
			T singleton = EntityQueryManagedComponentExtensions.GetSingleton<T>(query);
			query.Dispose();
			return singleton;
		}
	}

}
