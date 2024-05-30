using UnityEngine;

namespace Omniverse.FogOfWar.Rendering
{
	public static class ShaderVariables
	{
		public static int FogOfWarTexture { get; } = Shader.PropertyToID(nameof(FogOfWarTexture));
		public static int FogOfWarProperties { get; } = Shader.PropertyToID(nameof(FogOfWarProperties));
		public static int CellsVisibilityBuffer { get; } = Shader.PropertyToID(nameof(CellsVisibilityBuffer));
	}
}
