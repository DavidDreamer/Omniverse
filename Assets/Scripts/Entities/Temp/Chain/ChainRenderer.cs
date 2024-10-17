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
			LineRenderer.SetPosition(0, Chain.Owner.transform.position);

			int i = 1;
			foreach (var target in Chain.Targets)
			{
				LineRenderer.SetPosition(i, target.transform.position);
				i++;
			}
		}
	}
}
