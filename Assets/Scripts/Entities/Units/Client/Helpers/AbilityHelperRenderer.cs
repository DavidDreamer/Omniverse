using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Entities.Units.Client
{
	public class AbilityHelperRenderer: MonoBehaviour
	{
		[field: SerializeField, HideInInspector]
		public DecalProjector DecalProjector { get; private set; }

		private void OnValidate()
		{
			DecalProjector = GetComponent<DecalProjector>();
		}

		public void SetRange(float range)
		{
			float doubleRange = range * 2f;
			DecalProjector.size = new Vector3(doubleRange, doubleRange, DecalProjector.size.z);
		}
	}
}
