using Omniverse.Mapping;
using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Rendering
{
	[BurstCompile]
	public struct RenderSettings : IComponentData
	{
		public UnityObjectRef<SelectionBoxRenderSettings> SelectionBox;
		public UnityObjectRef<MinimapRenderSettings> Minimap;
		public UnityObjectRef<CursorRenderSettings> Cursor;
		public UnityObjectRef<FogOfWarRenderSettings> FogOfWar;
	}
}
