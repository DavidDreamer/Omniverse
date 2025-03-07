using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class EffectsBarWidget : MonoBehaviour, ITickable
	{
		[field: SerializeField]
		private RectTransform Holder { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		private UnitObsolete Unit { get; set; }

		private List<EffectWidget> EffectWdigets { get; } = new();

		public void Bind(UnitObsolete unit)
		{
			Unit = unit;
		}

		public void Tick()
		{
			for (int i = 0; i < Unit.Effects.Count; i++)
			{
				Effect effect = Unit.Effects[i];
				if (EffectWdigets.Count == i)
				{
					var effectWidget = ObjectResolver.Resolve<EffectWidget>();
					effectWidget.transform.SetParent(transform, false);
					EffectWdigets.Add(effectWidget);
				}

				EffectWdigets[i].Bind(effect);
			}

			for (int i = Unit.Effects.Count; i < EffectWdigets.Count; i++)
			{
				Destroy(EffectWdigets[i].gameObject);
				EffectWdigets.RemoveAt(i);
				i--;
			}

			foreach (EffectWidget effectWidget in EffectWdigets)
			{
				effectWidget.Tick();
			}

			Holder.ForceLayoutRebuild();
		}
	}
}
