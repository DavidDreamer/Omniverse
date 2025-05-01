using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class Widget : MonoBehaviour
	{
		protected EntityManager EntityManager { get; private set; }

		public virtual void Initialize(EntityManager entityManager)
		{
			EntityManager = entityManager;
		}

		public virtual void Tick()
		{
		}
	}
}
