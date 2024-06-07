using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.UI
{
	public class HotkeyWidget: MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		public void Set(InputAction inputAction)
		{
			InputBinding inputBinding = inputAction.bindings.First();
			string text = inputBinding.ToDisplayString();
			Label.text = text;
		}
	}
}
