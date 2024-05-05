using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Visibility
{
	public class Cell
	{
		public List<Cell> Neighbours;

		public bool Occluded;

		public Vector3 Position;

		public CellVisibilityState VisibilityState;

		public float Value;
	}
}
