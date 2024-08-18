using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Health Bar")]
	public class HealthBarsRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public int MaxCount { get; set; }

		[field: SerializeField]
		public DrawMeshParams DrawMeshParams { get; set; }

		[field: SerializeField]
		public Vector3 Offset { get; set; }

		[field: SerializeField]
		public HealthBarColors AllyColors { get; private set; }

		[field: SerializeField]
		public HealthBarColors EnemyColors { get; private set; }
	}
}
