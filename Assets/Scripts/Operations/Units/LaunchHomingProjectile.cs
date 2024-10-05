using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	public class LaunchHomingProjectile : Action
	{
		[field: SerializeField]
		public HomingProjectileDesc Projectile { get; private set; }

		public override async UniTask Perform(OperationContext context, CancellationToken token)
		{
			//TODO
			var unit = context.Actor as Unit;

			Vector3 position = context.Actor.transform.position;
			var homingProjectile = Object.Instantiate(Projectile.Model, position, Quaternion.identity).GetComponent<HomingProjectile>();
			homingProjectile.Initialize(Projectile);
			homingProjectile.ChangeFaction(unit.FactionID);
			context.ObjectResolver.Inject(homingProjectile);

			Unit target = context.Units().First();
			homingProjectile.Target = target;

			await UniTask.CompletedTask;
		}
	}
}