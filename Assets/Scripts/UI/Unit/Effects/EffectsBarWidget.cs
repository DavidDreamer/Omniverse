using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class EffectsBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private RectTransform Holder { get; set; }

		[field: SerializeField]
		private EffectWidget EffectWidget { get; set; }

		private List<EffectWidget> EffectWidgets { get; } = new();

		public void Tick(DynamicBuffer<Effect> effects)
		{
			for (int i = 0; i < effects.Length; i++)
			{
				Effect effect = effects[i];
				if (EffectWidgets.Count == i)
				{
					var effectWidget = Instantiate(EffectWidget);
					effectWidget.transform.SetParent(transform, false);
					EffectWidgets.Add(effectWidget);
				}
			}

			for (int i = effects.Length; i < EffectWidgets.Count; i++)
			{
				Destroy(EffectWidgets[i].gameObject);
				EffectWidgets.RemoveAt(i);
				i--;
			}

			for (int i = 0; i < effects.Length; i++)
			{
				EffectWidgets[i].Tick(effects[i]);
			}

			Holder.ForceLayoutRebuild();
		}
	}
}
