using UnityEngine.Scripting;

namespace Omniverse
{
	[Preserve]
	public class Player
	{
		public int FactionID { get; set; }
		
		public Unit Unit { get; set; }
	}
}
