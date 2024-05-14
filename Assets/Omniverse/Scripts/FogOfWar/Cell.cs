using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	public class Cell
	{
		public List<Cell> Neighbours;

		public bool Occluded;

		public Vector3 Position;

		public CellVisibilityState VisibilityState;
	}
}
