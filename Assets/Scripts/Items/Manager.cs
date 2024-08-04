using UnityEngine;
using VContainer;

namespace Omniverse.Items
{
	[UnityEngine.Scripting.Preserve]
	public class Manager
	{
		[Inject]
		private PrefabPool<Item> PrefabPool { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public void Spawn(ItemDesc desc, Vector3 position, Quaternion rotation)
		{
			Item presenter = PrefabPool.Take(desc.Prefab);
			Transform transform = presenter.transform;
			transform.SetPositionAndRotation(position, rotation);
		}

		// public void Consume(IConsumableItem item, CanvasScaler.Unit unit)
		// {
		// 	item.OnConsumed(unit);
		// 	PrefabPool.Return(item.Presenter);
		// }
	}
}
