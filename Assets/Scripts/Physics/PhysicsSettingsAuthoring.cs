using Dreambox.Core;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class PhysicsSettingsAuthoring : MonoBehaviour
	{
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

		public class Baker : Baker<PhysicsSettingsAuthoring>
		{
			public override void Bake(PhysicsSettingsAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new PhysicsSettings()
				{
					HitboxLayer = authoring.HitboxLayer,
					HitboxLayerMask = authoring.HitboxLayerMask
				});
			}
		}
	}
}
