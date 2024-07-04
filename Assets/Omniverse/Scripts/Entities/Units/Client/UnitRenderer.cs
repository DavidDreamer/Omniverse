using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Entities.Units.Client
{
	public class UnitRenderer: RendererComponent<Unit>
	{
		[field: SerializeField]
		private HealthBar HealthBar { get; set; }

		[field: SerializeField]
		public UnitMarker Selection { get; set; }

		[field: SerializeField]
		public UnitMarker Focus { get; set; }

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
			
			HealthBar.Initialize(unit);
			Selection.Initialize(unit);
			Focus.Initialize(unit);
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
		
		private void OnEffectApplied(Effect effect)
		{ 
			GameObject instance= Instantiate(effect.Desc.Prefab, transform, false);
			Effects.Add(effect,instance);
		}
		
		private void OnEffectRemoved(Effect effect)
		{
			Destroy(Effects[effect]);
			Effects.Remove(effect);
		}
		
		private void OnDied()
		{
			ProcessLivingState(false);
		}

		private void ProcessLivingState(bool alive)
		{
			HealthBar.gameObject.SetActive(alive);
		}
	}
}
