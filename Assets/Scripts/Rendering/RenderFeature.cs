using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	public class RenderFeature : MonoBehaviour
	{
		protected EntityManager EntityManager { get; private set; }

		public void Initialize(EntityManager entityManager)
		{
			EntityManager = entityManager;
			gameObject.SetActive(true);
		}
	}
}
