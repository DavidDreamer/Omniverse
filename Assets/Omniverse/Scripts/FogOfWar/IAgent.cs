using UnityEngine;

namespace Omniverse.FogOfWar
{
	public interface IAgent
	{
		int FactionID { get; }
		
		float VisionRange { get; }

		Vector3 Position { get; }
		
		Vector2Int Cell { get; set; }
		
		bool Visible { get; set; }
	}
}
