using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[UnityEngine.Scripting.Preserve]
	public class PrefabPool<T> where T: MonoBehaviour, IPoolObject
	{
		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		private Dictionary<T, Stack<T>> Items { get; } = new();

		private Dictionary<T, T> InstancePrefabPairs { get; } = new();
		
		public T Take(T prefab)
		{
			if (!Items.ContainsKey(prefab))
			{
				Items.Add(prefab, new Stack<T>());
			}

			if (Items[prefab].Count > 0)
			{
				T instance = Items[prefab].Pop();
				instance.gameObject.SetActive(true);
				return instance;
			}

			return CreateInstance(prefab);
		}

		private T CreateInstance(T prefab)
		{
			T instance = Object.Instantiate(prefab);
			ObjectResolver.InjectGameObject(instance.gameObject);
			InstancePrefabPairs.Add(instance, prefab);
			return instance;
		}
		
		public void Return(T instance)
		{
			instance.Cleanup();
			instance.gameObject.SetActive(false);
			T prefab = InstancePrefabPairs[instance];
			Items[prefab].Push(instance);
		}
	}
}
