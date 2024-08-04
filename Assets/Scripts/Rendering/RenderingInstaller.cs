using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dreambox.Rendering.URP;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Rendering")]
	public class RenderingInstaller: ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private UnitSelectorRenderingConfig UnitSelectorRenderingConfig { get; set; }

		[field: SerializeField]
		private HealthBarConfig HealthBarConfig { get; set; }

		public void Install(IContainerBuilder builder)
		{
			var universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
			ScriptableRenderer scriptableRenderer = universalRenderPipelineAsset.GetRenderer(0);
			PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var features = (List<ScriptableRendererFeature>)property.GetValue(scriptableRenderer);

			OutlineRendererFeature outline = features.OfType<OutlineRendererFeature>().First();
			builder.RegisterInstance(outline);

			builder.RegisterInstance(UnitSelectorRenderingConfig);
			builder.RegisterComponentOnNewGameObject<UnitSelectorRenderer>(Lifetime.Singleton, nameof(UnitSelectorRenderer)).AsImplementedInterfaces().AsSelf();

			builder.RegisterInstance(HealthBarConfig);
			builder.RegisterComponentOnNewGameObject<HealthBarRenderer>(Lifetime.Singleton, nameof(HealthBarRenderer)).AsImplementedInterfaces().AsSelf();

			builder.Register<Outliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		}
	}
}
