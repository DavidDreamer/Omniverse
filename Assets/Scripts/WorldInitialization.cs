using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class WorldInitialization : MonoBehaviour
	{
		public int SkipFrames;

		int framesSkipped;

		//public void Update()
		//{
		//	if (framesSkipped == SkipFrames)
		//	{
		//		DefaultWorldInitialization.Initialize("Default World", false);
		//		Destroy(gameObject);
		//		return;
		//	}

		//	framesSkipped++;
		//}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Init()
		{
			DefaultWorldInitialization.Initialize("Default World", false);
		}
	}
}
