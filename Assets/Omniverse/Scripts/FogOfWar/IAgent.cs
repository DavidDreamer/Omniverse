using UnityEngine;

namespace Omniverse.FogOfWar
{
	public interface IAgent
	{
		int FactionID { get; }
		
		float Range { get; }

		Vector3 Position { get; }
		
		Vector2Int Cell { get; internal set; }
		
		bool Visible { get; internal set; }
	}
}
