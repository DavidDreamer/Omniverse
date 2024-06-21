using Dreambox.Rendering;
using Dreambox.Rendering.URP;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Rendering")]
	public class RenderingInstaller: ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private OutlineConfig OutlineConfig { get; set; }

		public void Install(IContainerBuilder builder)
		{
			builder.Register<OutlinePass>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
				.WithParameter(OutlineConfig);
		}
	}
}
