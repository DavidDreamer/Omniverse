using UnityEngine;

namespace Omniverse.Units
{
	public class UnitFogOfWarAgent: FogOfWar.IAgent
	{
		private Unit Unit { get; }
		
		public int FactionID => Unit.FactionID;

		public float VisionRange => Unit.Desc.VisionRange;

		public Vector3 Position => Unit.Presenter.transform.position;

		public Vector2Int Cell { get; set; }

		public bool Visible { get; set; }
		
		public UnitFogOfWarAgent(Unit unit)
		{
			Unit = unit;
		}
	}
}
