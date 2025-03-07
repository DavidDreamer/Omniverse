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
}