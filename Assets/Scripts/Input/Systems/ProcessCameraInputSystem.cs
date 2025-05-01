using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessCameraInputSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			var selection = SystemAPI.GetSingleton<Selection>();

			if (selection.InProcess)
			{
				return;
			}

			Object.FindFirstObjectByType<CameraController>().Tick(state.EntityManager);
		}
	}
}
