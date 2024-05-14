using Omniverse.Units;
using UnityEngine.Scripting;

namespace Omniverse
{
	[Preserve]
	public class Player: IPlayer
	{
		public int FactionID { get; set; }
		
		public Unit Unit { get; set; }
	}
}
