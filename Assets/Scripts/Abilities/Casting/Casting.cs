using UnityEngine;

namespace Omniverse.Abilities
{
	public class Casting
	{
		//TODO
		public event System.Action<Casting> Started;

		private CastingDesc Desc { get; }

		public float Time { get; private set; }

		public float Factor { get; private set; }

		public bool InProcess { get; private set; }

		public bool Finished { get; private set; }

		public Casting(CastingDesc desc)
		{
			Desc = desc;
		}

		public void Start()
		{
			InProcess = true;
			Started?.Invoke(this);
		}

		public void Tick(float deltaTime)
		{
			Time += deltaTime;
			Factor = Mathf.Min(1f, Time / Desc.Time);
			Finished = Factor == 1f;
		}

		public void Reset()
		{
			InProcess = false;
			Time = 0;
		}
	}
}
