using System;
using System.Linq;
using UnityEngine;

namespace Omniverse.Units
{
	public class Attack
	{
		public event Action Started;
		
		private AttackDesc Desc { get; }
		
		public float Range { get; private set; }
		
		public float Speed { get; private set; }
		
		public float Damage { get; private set; }

		//TODO
		public float lastTime;
		
		public Attack(AttackDesc desc)
		{
			Desc = desc;

			Range = Desc.Range;
			Speed = Desc.Speed;
			Damage = Desc.Damage;
		}
		
		public void Perform(Unit target)
		{
			lastTime = Time.time;
			
			Started?.Invoke();
			
			var data = new ChangePropertyData
			{
				Amount = -Damage,
				ID = target.Properties.Keys.First()
			};
			
			target.ChangeResource(data);
		}
	}
}
