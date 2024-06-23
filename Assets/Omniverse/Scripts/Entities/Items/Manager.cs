using UnityEngine;
using VContainer;

namespace Omniverse.Entities.Items
{
	[UnityEngine.Scripting.Preserve]
	public class Manager
	{
		[Inject]
		private PrefabPool<ItemPresenter> PrefabPool { get; set; }
		
		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public void Spawn(ItemDesc desc, Vector3 position, Quaternion rotation)
		{
			Item item = desc.Build();
			ObjectResolver.Inject(item);

			ItemPresenter presenter = PrefabPool.Take(desc.Prefab);
			Transform transform = presenter.transform;
			transform.SetPositionAndRotation(position, rotation);
			
			presenter.Bind(item);
			item.Presenter = presenter;
		}

		// public void Consume(IConsumableItem item, CanvasScaler.Unit unit)
		// {
		// 	item.OnConsumed(unit);
		// 	PrefabPool.Return(item.Presenter);
		// }
	}
}
