using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class MissileAuthoring : MonoBehaviour
	{
		private class MissileBaker : Baker<MissileAuthoring>
		{
			public override void Bake(MissileAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Missile
				{
					Speed = 0
				});
			}
		}
	}
}
