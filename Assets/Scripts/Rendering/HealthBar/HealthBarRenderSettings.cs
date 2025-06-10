using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Health Bar")]
	public class HealthBarRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings MeshDrawSettings { get; set; }

		[field: SerializeField]
		public Vector3 Offset { get; set; }

		[field: SerializeField]
		public HealthBarColors AllyColors { get; private set; }

		[field: SerializeField]
		public HealthBarColors EnemyColors { get; private set; }
	}
}
