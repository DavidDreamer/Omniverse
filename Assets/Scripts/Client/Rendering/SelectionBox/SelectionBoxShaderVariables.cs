using UnityEngine;

namespace Omniverse.Rendering
{
	public static class SelectionBoxShaderVariables
	{
		public static int SelectionBox { get; } = Shader.PropertyToID(nameof(SelectionBox));
	}
}