using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dreambox.Math;
using JetBrains.Annotations;
using Omniverse.Entities.Units;
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
			Vector3 point = context.Points.First();
			Vector3 direction = new Vector3(point.x - position.x, 0, point.z - position.z).normalized;
			projectile.Direction = direction;
			await UniTask.CompletedTask;
		}
	}
}