using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.FogOfWar.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/FogOfWar")]
	public class FogOfWarRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public Shader PreProcessShader { get; private set; }

		[field: SerializeField]
		public Shader BlurShader { get; private set; }

		[field: SerializeField]
		public Shader ApplyShader { get; private set; }

		[field: SerializeField]
		public FogOfWarProperties Properties { get; private set; }

		[field: SerializeField]
		public BlurSettings BlurSettings { get; private set; }
	}
}
