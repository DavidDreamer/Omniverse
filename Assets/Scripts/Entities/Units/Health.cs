using Unity.Entities;

namespace Omniverse
{
	public struct Health : IComponentData
	{
		public float Maximum;

		public float Current;
	}
}
