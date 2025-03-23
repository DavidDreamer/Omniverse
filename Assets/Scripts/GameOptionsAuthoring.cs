using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class GameOptionsAuthoring : MonoBehaviour
	{
		[field: SerializeField]
		public GameOptions GameOptions { get; private set; }

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
