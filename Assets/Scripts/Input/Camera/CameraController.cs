using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct CameraController : IComponentData
	{
		public UnityObjectRef<CameraControllerSettings> Settings;

		public UnityObjectRef<Camera> Camera;

		public float Height;
	}
}
