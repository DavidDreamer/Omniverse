using UnityEngine;

namespace Omniverse.Entities.Units
{
	public class UnitFogOfWarAgent: FogOfWar.IAgent
	{
		private Unit Unit { get; }
		
		public int FactionID => Unit.FactionID;

		public float VisionRange => Unit.Properties.TryGetValue(PropertyID.VisionRange, out Property property)
			? property.Amount
			: 0;

		public Vector3 Position => Unit.Presenter.transform.position;

		public int CellIndex { get; set; }
		
		public UnitFogOfWarAgent(Unit unit)
		{
			Unit = unit;
		}
	}
}
