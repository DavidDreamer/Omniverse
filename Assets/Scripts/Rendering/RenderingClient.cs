using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class RenderingClient : MonoBehaviour
	{
		[field: SerializeField]
		public RenderFeature[] Features { get; set; }

		public void Initialize(EntityManager entityManager)
		{
			foreach (RenderFeature feature in Features)
			{
				feature.Initialize(entityManager);
			}
		}
	}
}
