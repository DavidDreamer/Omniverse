using UnityEngine;

namespace Omniverse
{
	public class UnitFogOfWarAgent : IFogOfWarAgentObsolete
	{
		private Unit Unit { get; }

		public int FactionID => Unit.FactionID;

		public float VisionRange => Unit.Properties.TryGetValue(PropertyID.VisionRange, out Property property)
			? property.Amount
			: 0;

		public Vector3 Position => Unit.transform.position;

		public int CellIndex { get; set; }

		public UnitFogOfWarAgent(Unit unit)
		{
			Unit = unit;
		}
	}
}
