using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public static class LayoutUtils
	{
		public static void ForceLayoutRebuild(this RectTransform rectTransform)
		{
			rectTransform.gameObject.SetActive(false);
			rectTransform.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		}
	}
}
