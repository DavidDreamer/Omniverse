using UnityEngine;
using VContainer;

namespace Omniverse.FogOfWar
{
	public class FogOfWarAgent: MonoBehaviour, IAgent
	{
		public int FactionID { get; set; }
		
		[field: SerializeField]
		public float Range { get; set; }

		public Vector3 Position => transform.position;

		Vector2Int IAgent.Cell { get; set; }

		bool IAgent.Visible { get; set; }
		
		[Inject]
		private FogOfWarManager Manager { get; set; }
		
		//TODO
		private void Start()
		{
			FactionID = GetComponent<UnitPresenter>().Unit.FactionID;
			Manager.Register(this);
		}

		private void OnDisable()
		{
			Manager.Unregister(this);
		}
	}
}
