using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Client
{
	public class BloodStain : MonoBehaviour
	{
		[field: SerializeField]
		private DecalProjector DecalProjector { get; set; }

		public void BeginAnimation()
		{
			//TODO ASYNC
			//AnimateAsync(destroyCancellationToken).Forget();

			//async UniTaskVoid AnimateAsync(CancellationToken token)
			//{
			//	float time = 0;

			//	const float animationTime = 1f;

			//	while (time < animationTime)
			//	{
			//		await UniTask.Yield(PlayerLoopTiming.Update, token);
			//		time += Time.deltaTime;
			//		time = Mathf.Min(time, animationTime);
			//		float animation = time / animationTime;
			//		DecalProjector.material.SetFloat("_Animation", animation);
			//	}
			//}
		}
	}
}
