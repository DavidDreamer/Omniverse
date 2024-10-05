using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
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

		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			var resourceSource = (ResourceSource)context.Entities.First();
			//TODO
			int factionID = ((Unit)context.Actor).FactionID;
			ResourceExtractionHadler.Extract(resourceSource, Amount, factionID);
			return UniTask.CompletedTask;
		}
	}
}
