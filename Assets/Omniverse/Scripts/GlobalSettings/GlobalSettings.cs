using Dreambox.Core;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Global Settings")]
	public class GlobalSettings: PreloadedScriptableObject<GlobalSettings>
	{
		[field: SerializeField]
		public ResourceDesc[] Resources { get; private set; }
		
		[field: SerializeField]
		public FactionDesc[] Factions { get; private set; }
		
		[field: Header("Physics")]
		[field: Layer]
		[field: SerializeField]
		public int HitboxLayer { get; private set; }
		
		[field: SerializeField]
		[field: HideInInspector]
		public LayerMask HitboxLayerMask { get; private set; }
		
#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();

			HitboxLayerMask = LayerMaskUtils.NumberToMask(HitboxLayer);
		}
#endif
	}
}
