using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	public class ExtractResourceDesc : IActionDesc
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public IAction Build() => new ExtractResource(this);
	}

	public class ExtractResource : Action<ExtractResourceDesc>
	{
		[Inject]
		private ResourceExtractionHadler ResourceExtractionHadler { get; set; }

		public ExtractResource(ExtractResourceDesc desc) : base(desc)
		{
		}

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			var resourceSource = (ResourceSource)context.Entities.First();
			//TODO
			int factionID = ((Unit)context.Caster).FactionID;
			ResourceExtractionHadler.Extract(resourceSource, Desc.Amount, factionID);
			return UniTask.CompletedTask;
		}
	}
}
