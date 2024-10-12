using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Units
{
	public class CastPointTargetAbilityCommand : CastAbilityCommand
	{
		private Vector3 Point { get; }

		public CastPointTargetAbilityCommand(Unit unit, Ability ability, Vector3 point) : base(unit, ability)
		{
			Point = point;
		}

		public override void Start()
		{
			base.Start();

			Ability.ActionContext.Points.Add(Point);
		}
	}
}
