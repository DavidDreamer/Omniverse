using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	public class LaunchHomingProjectileDesc : IActionDesc
	{
		[field: SerializeField]
		public HomingProjectileDesc Projectile { get; private set; }

		public async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO
			var unit = context.Caster as Unit;

			Vector3 position = context.Caster.transform.position;
			var homingProjectile = Object.Instantiate(Projectile.Model, position, Quaternion.identity).GetComponent<HomingProjectile>();
			homingProjectile.Initialize(Projectile);
			homingProjectile.ChangeFaction(unit.FactionID);

			Unit target = context.Units().First();
			homingProjectile.Target = target;

			await UniTask.CompletedTask;
		}
	}
}