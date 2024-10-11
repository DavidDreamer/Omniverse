using System.Collections.Generic;
using System.Reflection;
using Dreambox.Rendering;
using Dreambox.Rendering.Universal;
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
		private OutlineRendererConfig OutlineRendererConfig { get; set; }

		[field: SerializeField]
		private FogOfWarRendererConfig FogOfWarRendererConfig { get; set; }

		[field: SerializeField]
		private HealthBarsRendererConfig HealthBarsRendererConfig { get; set; }

		[field: SerializeField]
		private SelectionRendererConfig SelectionRendererConfig { get; set; }

		[field: SerializeField]
		private SelectionBoxRendererConfig SelectionBoxRendererConfig { get; set; }

		[field: SerializeField]
		private NavigationRendererConfig NavigationRendererConfig { get; set; }

		[field: SerializeField]
		private AbilityRendererConfig AbilityRendererConfig { get; set; }

		//TODO INJECT
		[field: SerializeField]
		private GameSettings GameSettings { get; set; }

		public void Install(IContainerBuilder builder)
		{
			var transform = new GameObject("Rendering").transform;

			var universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
			ScriptableRenderer scriptableRenderer = universalRenderPipelineAsset.GetRenderer(0);
			PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var features = (List<ScriptableRendererFeature>)property.GetValue(scriptableRenderer);

			RegisterRenderer<OutlineRenderer, OutlineRendererConfig>(OutlineRendererConfig);
			builder.Register<Outliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			if (GameSettings.FogOfWarConfig.Enabled)
			{
				RegisterRenderer<FogOfWarRenderer, FogOfWarRendererConfig>(FogOfWarRendererConfig);
			}

			RegisterRenderer<HealthBarsRenderer, HealthBarsRendererConfig>(HealthBarsRendererConfig);
			RegisterRenderer<SelectionRenderer, SelectionRendererConfig>(SelectionRendererConfig);
			RegisterRenderer<SelectionBoxRenderer, SelectionBoxRendererConfig>(SelectionBoxRendererConfig);
			RegisterRenderer<NavigationRenderer, NavigationRendererConfig>(NavigationRendererConfig);
			RegisterRenderer<AbilityRenderer, AbilityRendererConfig>(AbilityRendererConfig);

			void RegisterRenderer<TRenderer, TConfig>(TConfig config)
				where TRenderer : MonoBehaviour
			{
				builder.RegisterInstance(config);
				builder.RegisterComponentOnNewGameObject<TRenderer>(Lifetime.Singleton).UnderTransform(transform).AsImplementedInterfaces().AsSelf();
			}
		}
	}
}
