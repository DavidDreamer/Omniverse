using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using VContainer;

namespace Omniverse
{
	public class Chain : TempName
	{
		private float Time { get; set; }

		private ChainDesc Desc { get; set; }

		public Unit Target { get; set; }

		public Unit Owner { get; set; }

		[Inject]
		private PhysicsService PhysicsService { get; set; }

		public HashSet<Unit> Targets { get; } = new();

		public void Initialize(ChainDesc desc, int factionID)
		{
			Desc = desc;
			ChangeFaction(factionID);
		}

		public override void Tick(float deltaTime)
		{
			if (Time == 0)
			{
				Targets.Add(Target);
				Desc.Action.Perform(Owner, Target);
			}

			Time += deltaTime;

			if (Time >= Desc.BounceInterval)
			{
				var targets = PhysicsService.GetEntitiesInSphere<Unit>(Target.transform.position, Desc.BounceRange);
				Target = targets.FirstOrDefault(t => t != Owner && !Targets.Contains(t));

				if (Target == null)
				{
					Completed = true;
				}

				Time = 0;
			}
		}
	}
}
