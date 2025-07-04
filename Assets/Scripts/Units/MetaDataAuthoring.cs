using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class MetaDataAuthoring : MonoBehaviour
	{
		public string Name;

		public Sprite Icon;

		public class Baker : Baker<MetaDataAuthoring>
		{
			public override void Bake(MetaDataAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new MetaData()
				{
					Name = authoring.Name,
					Icon = authoring.Icon
				});
			}
		}
	}
}
