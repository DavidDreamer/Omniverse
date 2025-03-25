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
			//TODO ECS
			//LineRenderer.positionCount = Chain.Targets.Count + 1;

			//Vector3 ownerPosition = Chain.Owner.HitBox.transform.position;
			//LineRenderer.SetPosition(0, ownerPosition);

			//int i = 1;
			//foreach (var target in Chain.Targets)
			//{
			//	Vector3 position = target.HitBox.transform.position;
			//	LineRenderer.SetPosition(i, position);
			//	i++;
			//}
		}
	}
}
