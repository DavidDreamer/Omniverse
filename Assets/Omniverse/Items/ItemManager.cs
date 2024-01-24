using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[UnityEngine.Scripting.Preserve]
	public class ItemManager
	{
		[Inject]
		private PrefabPool<ItemPresenter> PrefabPool { get; set; }
		
		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public void Spawn(ItemDesc desc, Vector3 position, Quaternion rotation, Transform parent)
		{
			IItem item = desc.Build();
			ObjectResolver.Inject(item);

			ItemPresenter presenter = PrefabPool.Take(desc.Prefab);
			Transform transform = presenter.transform;
			transform.SetPositionAndRotation(position, rotation);
			transform.SetParent(parent);
			
			presenter.Item = item;
			item.Presenter = presenter;
		}

		public void Consume(IConsumableItem item, Unit unit)
		{
			item.OnConsumed(unit);
			PrefabPool.Return(item.Presenter);
		}
	}
}
