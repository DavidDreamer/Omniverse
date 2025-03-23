using Dreambox.Core;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class GameOptionsAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public GameOptions GameOptions { get; private set; }

		[field: Layer]
		[field: SerializeField]
		public int HitboxLayer { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public LayerMask HitboxLayerMask { get; private set; }

		private void OnValidate()
		{
			HitboxLayerMask = LayerMaskUtils.NumberToMask(HitboxLayer);
		}

		//private class Baker : Baker<GameOptionsAuthoring>
		//{
		//	public override void Bake(GameOptionsAuthoring authoring)
		//	{
		//		var entity = GetEntity(TransformUsageFlags.None);
		//		AddComponentObject(entity, authoring.GameOptions);
		//	}
		//}
	}
}
