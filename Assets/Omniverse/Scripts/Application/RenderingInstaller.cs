using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dreambox.Rendering.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Rendering")]
	public class RenderingInstaller: ScriptableObject, IInstaller
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
		}
	}
}
