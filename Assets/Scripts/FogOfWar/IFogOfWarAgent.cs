using UnityEngine;

namespace Omniverse
{
	public interface IFogOfWarAgent
	{
		int FactionID { get; }

		float VisionRange { get; }

		Vector3 Position { get; }

		int CellIndex { get; set; }
	}
}
