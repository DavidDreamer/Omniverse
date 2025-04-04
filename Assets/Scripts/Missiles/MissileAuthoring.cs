using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class MissileAuthoring : MonoBehaviour
	{
		public MissileDesc Desc;

		private class MissileBaker : Baker<MissileAuthoring>
		{
			public override void Bake(MissileAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Missile
				{
					Speed = authoring.Desc.Speed
				});
			}
		}
	}
}
