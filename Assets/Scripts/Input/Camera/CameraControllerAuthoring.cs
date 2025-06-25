using Unity.Entities;
using UnityEngine;

namespace Omniverse.Input
{
	public class CameraControllerAuthoring : MonoBehaviour
	{
		public CameraControllerSettings Settings;

		private class Baker : Baker<CameraControllerAuthoring>
		{
			public override void Bake(CameraControllerAuthoring authoring)
			{
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new CameraController()
				{
					Settings = authoring.Settings
				});
			}
		}
	}
}
