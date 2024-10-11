using System.Linq;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	public class ExtractResource : Action
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		[Inject]
		private ResourceExtractionHadler ResourceExtractionHadler { get; set; }

		public override void PerformTemp(OperationContext context)
		{
			var resourceSource = (ResourceSource)context.Entities.First();
			//TODO
			int factionID = ((Unit)context.Actor).FactionID;
			ResourceExtractionHadler.Extract(resourceSource, Amount, factionID);
		}
	}
}
