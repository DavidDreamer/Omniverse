using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Units
{
	public class CastDirectionalAbilityCommand : CastAbilityCommand
	{
		public Vector3 Direction { get; }

		public CastDirectionalAbilityCommand(Unit unit, Ability ability, Vector3 direction) : base(unit, ability)
		{
			Direction = direction;
		}

		public override void Start()
		{
			base.Start();

			Ability.OperationContext.Directions.Add(Direction);
		}
	}
}
