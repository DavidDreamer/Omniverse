using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Health Bar")]
	public class HealthBarRenderConfig : ScriptableObject
	{
		[field: SerializeField]
		public RenderPassEvent RenderPassEvent { get; set; }

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
