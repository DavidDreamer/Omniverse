using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class LaunchProjectile : Action<LaunchProjectileDesc>
	{
		public LaunchProjectile(LaunchProjectileDesc desc) : base(desc)
		{
		}

		public override async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO
			var unit = context.Caster as Unit;

			Vector3 position = context.Caster.transform.position;
			Projectile projectile = Object.Instantiate(Desc.Projectile.Model, position, Quaternion.identity).GetComponent<Projectile>();
			projectile.Initialize(Desc.Projectile);
			projectile.ChangeFaction(unit.FactionID);
			Vector3 direction = context.Directions.First();
			projectile.Direction = direction;
			await UniTask.CompletedTask;
		}
	}
}