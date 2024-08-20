using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using VContainer;
using Omniverse.FogOfWar;

namespace Omniverse.Rendering.FogOfWar
{
	public class FogOfWarRenderer : CustomRenderer<FogOfWarRendererConfig, FogOfWarPass>
	{
		private static class ShaderVariables
		{
			public const string ExploredKeyword = "FOG_OF_WAR_EXPLORED";

			public static int FogOfWarResolution { get; } = Shader.PropertyToID(nameof(FogOfWarResolution));

			public static int FogOfWarTexture { get; } = Shader.PropertyToID(nameof(FogOfWarTexture));

			public static int FogOfWarProperties { get; } = Shader.PropertyToID(nameof(FogOfWarProperties));

			public static int CellsVisibilityBuffer { get; } = Shader.PropertyToID(nameof(CellsVisibilityBuffer));
		}

		[Inject]
		public Manager Manager { get; set; }

		private Material AnimationMaterial { get; set; }

		private Material BlurMaterial { get; set; }

		private RenderTexture AnimationRT { get; set; }

		public RenderTexture BlurRT1 { get; set; }

		public RenderTexture BlurRT2 { get; set; }

		private ConstantComputeBuffer<FogOfWarProperties> PropertiesBuffer { get; set; }

		private ComputeBuffer CellsVisibilityBuffer { get; set; }

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				UpdateShaderVariables();
			}
		}

		private void UpdateShaderVariables()
		{
			PropertiesBuffer?.SetData(Config.Properties);

			if (BlurMaterial != null)
			{
				Config.BlurSettings.ApplyTo(BlurMaterial);
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			var resolution = new Vector4(Manager.Resolution.x, Manager.Resolution.y);
			Shader.SetGlobalVector(ShaderVariables.FogOfWarResolution, resolution);

			AnimationMaterial = new Material(Config.PreProcessShader);
			BlurMaterial = new Material(Config.BlurShader);

			AnimationRT = CreateAnimationRT("FogOfWar.Animation");
			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			PropertiesBuffer = new ConstantComputeBuffer<FogOfWarProperties>(ShaderVariables.FogOfWarProperties);

			CellsVisibilityBuffer =
				new ComputeBuffer(Manager.CellsVisibilityPerFaction[0].Length, sizeof(CellVisibilityState));

			if (Manager.Settings.Explored)
			{
				Shader.EnableKeyword(ShaderVariables.ExploredKeyword);
			}
			else
			{
				Shader.DisableKeyword(ShaderVariables.ExploredKeyword);
			}

			UpdateShaderVariables();

			RenderTexture CreateAnimationRT(string textureName)
			{
				return new RenderTexture(Manager.Resolution.x, Manager.Resolution.y,
					GraphicsFormat.R16G16_SNorm,
					GraphicsFormat.None)
				{
					name = textureName,
					filterMode = FilterMode.Point,
					anisoLevel = 0
				};
			}

			RenderTexture CreateBlurRT(string textureName)
			{
				return new RenderTexture(Manager.Resolution.x, Manager.Resolution.y,
					GraphicsFormat.R16G16_SFloat,
					GraphicsFormat.None)
				{
					name = textureName,
					anisoLevel = 0
				};
			}
		}

		protected override FogOfWarPass CreatePass() => new FogOfWarPass(this);

		protected override bool IsInactive() => false;

		public override void Dispose()
		{
			base.Dispose();

			CoreUtils.Destroy(AnimationMaterial);
			CoreUtils.Destroy(BlurMaterial);

			AnimationRT.Release();
			BlurRT1.Release();
			BlurRT2.Release();

			PropertiesBuffer.Dispose();
			CellsVisibilityBuffer.Release();
		}

		private void LateUpdate()
		{
			using var scope = new CommandBufferScope("FogOfWar.PreProcess");
			CommandBuffer commandBuffer = scope.CommandBuffer;

			commandBuffer.SetBufferData(CellsVisibilityBuffer, Manager.CellsVisibilityPerFaction[0]);
			commandBuffer.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			commandBuffer.Blit(AnimationRT, AnimationRT, AnimationMaterial, 0);
			commandBuffer.Blit(AnimationRT, BlurRT1, BlurMaterial, 0);
			commandBuffer.Blit(BlurRT1, BlurRT2, BlurMaterial, 1);

			commandBuffer.SetGlobalTexture(ShaderVariables.FogOfWarTexture, BlurRT2);
		}
	}
}
