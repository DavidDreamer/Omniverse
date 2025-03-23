using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class WorldInitialization : MonoBehaviour
	{
		//[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public void Awake()
		{
			DefaultWorldInitialization.Initialize("Default World", false);
		}
	}
}
