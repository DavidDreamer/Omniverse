using System;
using UnityEngine;

namespace Omniverse
{
	[Obsolete]
	public interface IFogOfWarAgentObsolete
	{
		int FactionID { get; }

		float VisionRange { get; }

		Vector3 Position { get; }

		int CellIndex { get; set; }
	}
}
