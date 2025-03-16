using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class PrefabPool<T> where T : MonoBehaviour, IPoolObject
	{
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
