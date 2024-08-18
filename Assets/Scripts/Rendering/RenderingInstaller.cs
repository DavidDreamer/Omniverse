using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dreambox.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class RenderingInstaller : MonoBehaviour, IInstaller
	{
		public void Install(IContainerBuilder builder)
		{
			var universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
			ScriptableRenderer scriptableRenderer = universalRenderPipelineAsset.GetRenderer(0);
			PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var features = (List<ScriptableRendererFeature>)property.GetValue(scriptableRenderer);

			OutlineRendererFeature outline = features.OfType<OutlineRendererFeature>().First();
			builder.RegisterInstance(outline);
			builder.Register<Outliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterComponentInHierarchy<HealthBarsRenderer>().AsImplementedInterfaces().AsSelf();
			builder.RegisterComponentInHierarchy<SelectionRenderer>().AsImplementedInterfaces().AsSelf();
			builder.RegisterComponentInHierarchy<NavigationRenderer>().AsImplementedInterfaces().AsSelf();
		}
	}
}
