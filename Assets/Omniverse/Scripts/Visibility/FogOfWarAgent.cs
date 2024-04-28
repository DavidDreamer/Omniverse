using UnityEngine;

namespace Omniverse.Visibility
{
	public class FogOfWarAgent: MonoBehaviour
	{
		public int FactionID;
		
		public float Range;

		public FogOfWarCell Cell;

		//TODO
		private void Start()
		{
			FactionID = GetComponent<UnitPresenter>().Unit.FactionID;
		}
	}
}
