using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	public class LaunchHomingProjectile : Action<LaunchHomingProjectileDesc>
	{
		public LaunchHomingProjectile(LaunchHomingProjectileDesc desc) : base(desc)
		{
		}

		public override async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO
			var unit = context.Caster as Unit;

			Vector3 position = context.Caster.transform.position;
			var homingProjectile = Object.Instantiate(Desc.Projectile.Model, position, Quaternion.identity).GetComponent<HomingProjectile>();
			homingProjectile.Initialize(Desc.Projectile);
			homingProjectile.ChangeFaction(unit.FactionID);

			Unit target = context.Units().First();
			homingProjectile.Target = target;

			await UniTask.CompletedTask;
		}
	}
}