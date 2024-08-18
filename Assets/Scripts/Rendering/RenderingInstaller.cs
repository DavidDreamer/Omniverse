using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dreambox.Rendering.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Rendering")]
	public class RenderingInstaller : ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private SelectionRenderConfig SelectionRenderConfig { get; set; }

		[field: SerializeField]
		private HealthBarRenderConfig HealthBarRenderConfig { get; set; }

		[field: SerializeField]
		private NavigationRenderConfig NavigationRenderConfig { get; set; }

		public void Install(IContainerBuilder builder)
		{
			var universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
			ScriptableRenderer scriptableRenderer = universalRenderPipelineAsset.GetRenderer(0);
			PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var features = (List<ScriptableRendererFeature>)property.GetValue(scriptableRenderer);

			OutlineRendererFeature outline = features.OfType<OutlineRendererFeature>().First();
			builder.RegisterInstance(outline);

			NavigationRendererFeature navigation = features.OfType<NavigationRendererFeature>().First();
			builder.RegisterInstance(navigation);
			builder.Register<Navigator>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterInstance(SelectionRenderConfig);
			builder.RegisterComponentOnNewGameObject<SelectionRenderer>(Lifetime.Singleton, nameof(SelectionRenderer)).AsImplementedInterfaces().AsSelf();

			builder.Register<Outliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterInstance(HealthBarRenderConfig);
			builder.RegisterComponentOnNewGameObject<HealthBarRenderer>(Lifetime.Singleton, nameof(HealthBarRenderer)).AsImplementedInterfaces().AsSelf();
		}
	}
}
