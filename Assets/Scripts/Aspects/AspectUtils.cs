using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Omniverse
{
	public static class AspectUtils
	{
		public static bool HasAspect<T>(this EntityManager entityManager, Entity entity) where T : unmanaged, IAspect, IAspectCreate<T>
		{
			var create = new T();

			var list = new UnsafeList<ComponentType>(10, Allocator.Temp);
			create.AddComponentRequirementsTo(ref list);

			bool result = true;

			foreach (ComponentType component in list)
			{
				if (!entityManager.HasComponent(entity, component))
				{
					result = false;
					break;
				}
			}

			list.Dispose();

			return result;
		}
	}
}
