using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ExtractResource : Action
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public override void Perform(ActionContext context)
		{
			var resourceSource = (ResourceSource)context.Entities.First();
			//TODO
			int factionID = ((Unit)context.Actor).FactionID;
			context.ResourceExtractionHadler.Extract(resourceSource, Amount, factionID);
		}
	}
}
