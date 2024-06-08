using System;
using System.Linq;
using UnityEngine;

namespace Omniverse.Units
{
	public class Attack
	{
		public event Action Started;
		
		private Unit Unit { get; }
		
		//TODO
		public float lastTime;
		
		public Attack(Unit unit)
		{
			Unit = unit;
		}
		
		public void Perform(Unit target)
		{
			lastTime = Time.time;
			
			Started?.Invoke();
			
			var data = new ChangePropertyData
			{
				Amount = -Unit.Properties[PropertyID.AttackDamage].Amount,
				ID = target.Properties.Keys.First()
			};
			
			target.ChangeResource(data);
		}
	}
}
