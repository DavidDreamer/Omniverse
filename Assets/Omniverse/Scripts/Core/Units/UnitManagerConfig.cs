using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Unit Manager", fileName = nameof(UnitManagerConfig))]
	public class UnitManagerConfig: ScriptableObject
	{
		[field: SerializeField]
		public UnitPresenter UnitPresenter { get; private set; }
		
		[field: SerializeField]
		public float DespawnDelay { get; private set; }
	}
}
