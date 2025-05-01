using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class UIHandler : MonoBehaviour
	{
		[field: SerializeField]
		private List<Widget> Widgets { get; set; }

		public void Initialize(EntityManager entityManager)
		{
			foreach (Widget widget in Widgets)
			{
				widget.Initialize(entityManager);
			}
		}

		public void Tick()
		{
			foreach (Widget widget in Widgets)
			{
				widget.Tick();
			}
		}
	}
}
