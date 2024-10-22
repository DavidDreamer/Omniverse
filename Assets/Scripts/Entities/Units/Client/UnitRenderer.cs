using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Client
{
	public class UnitRenderer : RendererComponent<Unit>
	{
		[field: SerializeField]
		private BloodStain BloodStain { get; set; }

		[field: SerializeField]
		public MeshFilter[] MeshFilters { get; private set; }

		[field: SerializeField]
		public AudioSource AudioSource { get; private set; }

		private Dictionary<Effect, GameObject> Effects { get; } = new();

		public override void Initialize(Unit unit)
		{
			base.Initialize(unit);

			Entity.Died += OnDied;
			Entity.EffectApplied += OnEffectApplied;
			Entity.EffectRemoved += OnEffectRemoved;
		}

		private void OnDestroy()
		{
			if (Entity == null)
			{
				return;
			}

			Entity.Died -= OnDied;
			Entity.EffectApplied += OnEffectApplied;
			Entity.EffectRemoved += OnEffectRemoved;
		}

		private void OnDied()
		{
			BloodStain bloodStain = Instantiate(BloodStain);
			bloodStain.transform.position = transform.position;
			bloodStain.BeginAnimation();
		}

		private void OnEffectApplied(Effect effect)
		{
			if (effect.Desc.Prefab == null)
			{
				return;
			}

			GameObject instance = Instantiate(effect.Desc.Prefab, Entity.HitBox.transform.position, Quaternion.identity, transform);
			Effects.Add(effect, instance);
		}

		private void OnEffectRemoved(Effect effect)
		{
			if (effect.Desc.Prefab == null)
			{
				return;
			}

			Destroy(Effects[effect]);
			Effects.Remove(effect);
		}
	}
}
