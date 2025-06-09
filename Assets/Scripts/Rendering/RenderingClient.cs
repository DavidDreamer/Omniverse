using Omniverse.Mapping;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class RenderingClient : MonoBehaviour
	{
		[field: SerializeField]
		public SelectionBoxRendererConfig SelectionBoxRendererConfig { get; set; }

		[field: SerializeField]
		public MinimapRenderConfig MinimapRenderConfig { get; set; }

		[field: SerializeField]
		public CursorRendererConfig CursorRendererConfig { get; set; }
	}
}
