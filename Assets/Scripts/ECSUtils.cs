using Unity.Entities;

public static class ECSUtils
{
	public static T GetSingleton<T>() where T : unmanaged, IComponentData
	{
		var query = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
		var singleton = query.GetSingleton<T>();
		query.Dispose();
		return singleton;
	}

	public static T GetSingletonManaged<T>() where T : class, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
		T singleton = EntityQueryManagedComponentExtensions.GetSingleton<T>(query);
		query.Dispose();
		return singleton;
	}
}