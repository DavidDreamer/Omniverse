using UnityEngine;

namespace Omniverse
{
	public class ItemPresenter: MonoBehaviour, IPoolObject
	{
		public IItem Item { get; set; }

		public virtual void Cleanup()
		{
		}
	}
}
