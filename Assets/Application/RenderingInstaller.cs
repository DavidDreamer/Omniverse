using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dreambox.Rendering.Universal;
using Omniverse.Rendering.FogOfWar;
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
		private FogOfWarRendererConfig FogOfWarRendererConfig { get; set; }

		[field: SerializeField]
		private HealthBarsRendererConfig HealthBarsRendererConfig { get; set; }

		[field: SerializeField]
		private SelectionRendererConfig SelectionRendererConfig { get; set; }

		[field: SerializeField]
		private NavigationRendererConfig NavigationRendererConfig { get; set; }

		[Inject]
		private GameSettings GameSettings { get; set; }

		public void Install(IContainerBuilder builder)
		{
			var transform = new GameObject("Rendering").transform;

			var universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
			ScriptableRenderer scriptableRenderer = universalRenderPipelineAsset.GetRenderer(0);
			PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var features = (List<ScriptableRendererFeature>)property.GetValue(scriptableRenderer);

			OutlineRendererFeature outline = features.OfType<OutlineRendererFeature>().First();
			builder.RegisterInstance(outline);
			builder.Register<Outliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			if (GameSettings.FogOfWarSettings.Enabled)
			{
				RegisterRenderer<FogOfWarRenderer, FogOfWarRendererConfig>(FogOfWarRendererConfig);
			}

			RegisterRenderer<HealthBarsRenderer, HealthBarsRendererConfig>(HealthBarsRendererConfig);
			RegisterRenderer<SelectionRenderer, SelectionRendererConfig>(SelectionRendererConfig);
			RegisterRenderer<NavigationRenderer, NavigationRendererConfig>(NavigationRendererConfig);

			void RegisterRenderer<TRenderer, TConfig>(TConfig config)
				where TRenderer : MonoBehaviour
			{
				builder.RegisterInstance(config);
				builder.RegisterComponentOnNewGameObject<TRenderer>(Lifetime.Singleton).UnderTransform(transform).AsImplementedInterfaces().AsSelf();
			}
		}
	}
}
