using UnityEngine;

namespace Omniverse.Abilities
{
	public struct Casting
	{
		public float Time;

		public float CurrentTime;

		public bool InProcess;

		public float Factor => Mathf.Min(1f, CurrentTime / Time);

		public bool Finished => Factor == 1f;

		public void Start()
		{
			InProcess = true;
		}

		public void Tick(float deltaTime)
		{
			CurrentTime += deltaTime;
		}

		public void Reset()
		{
			InProcess = false;
			CurrentTime = 0;
		}
	}
}
