using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	[RequireComponent(typeof(Chain))]
	public class ChainRenderer : MonoBehaviour
	{
		[field: SerializeField]
		[field: HideInInspector]
		private Chain Chain { get; set; }

		[field: SerializeField]
		private LineRenderer LineRenderer { get; set; }

		private void OnValidate()
		{
			Chain = GetComponent<Chain>();
		}

		private void LateUpdate()
		{
			LineRenderer.positionCount = Chain.Targets.Count + 1;

			var ownerRenderer = Chain.Owner.GetComponentInChildren<RendererComponent<Unit>>();
			LineRenderer.SetPosition(0, ownerRenderer.Center.position);

			int i = 1;
			foreach (var target in Chain.Targets)
			{
				var renderer = target.GetComponentInChildren<RendererComponent<Unit>>();
				Vector3 position = renderer.Center.position;
				LineRenderer.SetPosition(i, position);
				i++;
			}
		}
	}
}
