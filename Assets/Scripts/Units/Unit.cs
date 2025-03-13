using Unity.Entities;

namespace Omniverse
{

	public class Unit : IComponentData
	{
		public UnitDesc Desc { get; set; }
	}
}
