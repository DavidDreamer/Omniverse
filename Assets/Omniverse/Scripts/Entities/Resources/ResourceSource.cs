using UnityEngine;

namespace Omniverse
{
	public class ResourceSource: EntityPresenter
	{
		[field: SerializeField]
		public ResourceSourceDesc Desc { get; private set; }
	}
}
