using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class ResourceExtractionHadler
	{
		[Inject]
		private FactionManager FactionManager { get; set; }
		
		public void Extract(IEntity entity, ResourceSource resourceSource, int amount)
		{
			amount = Mathf.Min(resourceSource.Amount, amount);
			resourceSource.ChangeAmount(-amount);
			int factionID = entity.FactionID;
			FactionManager.Factions[factionID].ChangeResource(resourceSource.Desc.Resource, amount);
		}
	}
}
