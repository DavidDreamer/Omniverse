using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ApplyEffect : Action
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public override void PerformTemp(OperationContext context)
		{
			foreach (var unit in context.Units())
			{
				var effect = new Effect(Effect);

				unit.ApplyEffect(effect);
			}
		}
	}
}
