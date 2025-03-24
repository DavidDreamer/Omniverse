using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class CastingWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Image { get; set; }

		public void Tick(Casting casting)
		{
			bool inProcess = casting.InProcess;
			Image.enabled = inProcess;

			if (inProcess)
			{
				Image.fillAmount = casting.Factor;
			}
		}
	}
}
