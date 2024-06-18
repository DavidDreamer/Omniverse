using Omniverse.Items;
using UnityEngine;
using VContainer;

namespace Omniverse.Units
{
	public class SpawnItemEvent: MonoBehaviour
	{
		[field: SerializeField]
		private ItemDesc Desc { get; set; }

		[Inject]
		private Items.Manager ItemsManager { get; set; }
		
		public void Start()
		{
			ItemsManager.Spawn(Desc, transform.position, transform.rotation);
		}
	}
}
