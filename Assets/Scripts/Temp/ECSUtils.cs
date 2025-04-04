using System;
using Unity.Entities;

public static class ECSUtils
{
	public static World ClientWorld
	{
		get
		{
			foreach (World world in World.All)
			{
				if (world.Name == "ClientWorld")
				{
					return world;
				}
			}

			throw new Exception("ClientWorld not found.");
		}
	}

	public static T GetSingleton<T>() where T : unmanaged, IComponentData
	{
		var query = ClientWorld.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
		var singleton = query.GetSingleton<T>();
		query.Dispose();
		return singleton;
	}

	public static T GetSingletonManaged<T>() where T : class, IComponentData
	{
		var entityManager = ClientWorld.EntityManager;
		var query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(T) });
		T singleton = EntityQueryManagedComponentExtensions.GetSingleton<T>(query);
		query.Dispose();
		return singleton;
	}
}