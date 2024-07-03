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

		public override void Initialize(Unit unit)
		{
			base.Initialize(unit);

			unit.Died += OnDied;

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
