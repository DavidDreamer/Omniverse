using UnityEngine;

namespace Omniverse
{
	public class ItemPresenter: MonoBehaviour, IPoolObject<ItemPresenter>
	{
		public IItem Item { get; set; }

		public ItemPresenter Prefab { get; set; }

		public virtual void Cleanup()
		{
		}
	}
}
