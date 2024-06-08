using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class EffectsBarWidget: MonoBehaviour, ITickable
	{
		[field: SerializeField]
		private RectTransform Holder { get; set; }
		
		[Inject]
		private IObjectResolver ObjectResolver { get; set; }
		
		private Unit Unit { get; set; }

		private List<EffectWidget> EffectIndicators { get; } = new();

		public void Bind(Unit unit)
		{
			Unit = unit;
		}

		public void Tick()
		{
			for (int i = 0; i < Unit.Effects.Count; i++)
			{
				Effect effect = Unit.Effects[i];
				if (EffectIndicators.Count == i)
				{
					var effectIndicator = ObjectResolver.Resolve<EffectWidget>();
					effectIndicator.transform.SetParent(transform, false);
					EffectIndicators.Add(effectIndicator);
				}

				EffectIndicators[i].Bind(effect);
			}

			for (int i = Unit.Effects.Count; i < EffectIndicators.Count; i++)
			{
				Destroy(EffectIndicators[i].gameObject);
				EffectIndicators.RemoveAt(i);
				i--;
			}
			
			foreach (EffectWidget effectIndicator in EffectIndicators)
			{
				effectIndicator.Tick();
			}
			
			Holder.ForceLayoutRebuild();
		}
	}
}
