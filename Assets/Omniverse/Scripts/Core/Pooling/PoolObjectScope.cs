using System;
using UnityEngine;

namespace Omniverse
{
	public readonly struct PoolObjectScope<T>: IDisposable where T: MonoBehaviour, IPoolObject
	{
		private PrefabPool<T> Pool { get; }
	
		public T Instance { get; }
	
		public PoolObjectScope(PrefabPool<T> pool, T prefab)
		{
			Pool = pool;
			Instance = pool.Take(prefab);
		}

		public void Dispose()
		{
			if (Instance == null)
			{
				return;
			}
		
			Pool.Return(Instance);
		}
	}
}
