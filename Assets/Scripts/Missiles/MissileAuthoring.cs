using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

namespace Omniverse
{
	public class OnDestroyTrigger : IComponentData
	{
		public VisualEffect Prefab;
	}

	public class MissileAuthoring : MonoBehaviour
	{
		public MissileDesc Desc;

		public VisualEffect OnDestroyEffect;

		private class MissileBaker : Baker<MissileAuthoring>
		{
			public override void Bake(MissileAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Missile
				{
					Speed = authoring.Desc.Speed
				});

				if (authoring.OnDestroyEffect != null)
				{
					AddComponentObject(entity, new OnDestroyTrigger
					{
						Prefab = authoring.OnDestroyEffect
					});
				}
			}
		}
	}
}
