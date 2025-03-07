using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class FogOfWarObstacleAuthoring : MonoBehaviour
	{
		public Vector3 Size;

		private class Baker : Baker<FogOfWarObstacleAuthoring>
		{
			public override void Bake(FogOfWarObstacleAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new FogOfWarObstacle()
				{
					Size = authoring.Size
				});
			}
		}
	}
}
