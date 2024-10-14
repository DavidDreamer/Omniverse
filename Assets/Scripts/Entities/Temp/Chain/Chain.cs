using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using VContainer;

namespace Omniverse
{
	public class Chain : TempName, IFactious
	{
		private float Time { get; set; }

		private ChainDesc Desc { get; set; }

		public Unit Target { get; set; }

		public Unit Owner { get; set; }

		public int FactionID { get; set; }

		[Inject]
		private ActionHandler ActionHandler { get; set; }

		[Inject]
		private PhysicsService PhysicsService { get; set; }

		private HashSet<Unit> Targets { get; } = new();

		public void Initialize(ChainDesc desc)
		{
			Desc = desc;
		}

		public override void Tick(float deltaTime)
		{
			if (Time == 0)
			{
				Targets.Add(Target);
				var contex = new ActionContext(Owner);
				contex.Entities.Add(Target);
				ActionHandler.Perform(Desc.Action, Owner, contex);
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
