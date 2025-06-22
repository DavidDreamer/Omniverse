using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public static class WorldInitialization
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Initialize()
		{
			DefaultWorldInitialization.Initialize("Default World", false);
		}
	}
}
