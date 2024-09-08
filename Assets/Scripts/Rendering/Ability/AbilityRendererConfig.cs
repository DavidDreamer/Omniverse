using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Ability")]
	public class AbilityRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public DrawMeshParams Range { get; private set; }
	}
}
