using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class FogOfWarAgentAuthoring : MonoBehaviour
	{
		public float VisionRange;

		private class Baker : Baker<FogOfWarAgentAuthoring>
		{
			public override void Bake(FogOfWarAgentAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new FogOfWarAgent()
				{
					VisionRange = authoring.VisionRange
				});
			}
		}
	}
}
