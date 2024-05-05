using UnityEngine;

namespace Omniverse.Visibility
{
	public class FogOfWarAgent: MonoBehaviour
	{
		public int FactionID;
		
		public float Range;

		public Vector2Int Cell;

		//TODO
		private void Start()
		{
			FactionID = GetComponent<UnitPresenter>().Unit.FactionID;
		}
	}
}
