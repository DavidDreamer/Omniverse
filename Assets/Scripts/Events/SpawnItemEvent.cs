using Omniverse.Items;
using UnityEngine;
using VContainer;

namespace Omniverse.Events
{
	public class SpawnItemEvent : MonoBehaviour
	{
		[field: SerializeField]
		private ItemDesc Desc { get; set; }

		[Inject]
		private ItemManager ItemsManager { get; set; }

		public void Start()
		{
			ItemsManager.Spawn(Desc, transform.position, transform.rotation);
		}
	}
}
