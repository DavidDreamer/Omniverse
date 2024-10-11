using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class CastingWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Image { get; set; }

		private Abilities.Casting Casting { get; set; }

		public void Bind(Abilities.Casting casting)
		{
			Casting = casting;
		}

		public void Tick()
		{
			bool inProcess = Casting.InProcess;
			Image.enabled = inProcess;

			if (inProcess)
			{
				Image.fillAmount = Casting.Factor;
			}
		}
	}
}
