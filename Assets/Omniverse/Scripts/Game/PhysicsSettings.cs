using Dreambox.Core;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Physics")]
	public class PhysicsSettings: ScriptableObject
	{
		[field: Header("Physics")]
		[field: Layer]
		[field: SerializeField]
		public int HitboxLayer { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public LayerMask HitboxLayerMask { get; private set; }

		private void OnValidate()
		{
			HitboxLayerMask = LayerMaskUtils.NumberToMask(HitboxLayer);
		}
	}
}
