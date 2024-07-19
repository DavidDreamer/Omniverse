using Dreambox.Math;
using UnityEngine;

namespace Omniverse.Entities.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Projectile")]
	public class ProjectileDesc: ScriptableObject
	{
		[field: SerializeField]
		private Projectile Prefab { get; set; }

		private Projectile Presenter { get; set; }

		public void InstantiatePresenter(Vector3 position, Quaternion rotation)
		{
			Presenter = Instantiate(Prefab, position, rotation);
		}

		public void Launch(ParabolicTrajectory3D trajectory, Vector3 direction, float force) =>
			Presenter.Launch(trajectory, direction, force);
	}
}
