using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Chain")]
	public class ChainDesc : EntityDesc
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		[field: SerializeField]
		public int MaxTargets { get; private set; }

		[field: SerializeField]
		public float BounceRange { get; private set; }

		[field: SerializeField]
		public float BounceInterval { get; private set; }

		[field: SerializeReference]
		public IOperation<Entity> Operation { get; private set; }
	}
}
