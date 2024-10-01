using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	public class LaunchProjectileDesc : IActionDesc
	{
		[field: SerializeField]
		public ProjectileDesc Projectile { get; private set; }

		public async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO
			var unit = context.Caster as Unit;

			Vector3 position = context.Caster.transform.position;
			Projectile projectile = Object.Instantiate(Projectile.Model, position, Quaternion.identity).GetComponent<Projectile>();
			projectile.Initialize(Projectile);
			projectile.ChangeFaction(unit.FactionID);
			Vector3 direction = context.Directions.First();
			projectile.Direction = direction;
			await UniTask.CompletedTask;
		}
	}
}