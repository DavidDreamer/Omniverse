using System;
using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class FogOfWarRenderer : CustomRenderer<FogOfWarRendererConfig, FogOfWarPass>, IInitializable, IDisposable
	{
		private static class ShaderVariables
		{
			public const string ExploredKeyword = "FOG_OF_WAR_EXPLORED";

			public static int FogOfWarResolution { get; } = Shader.PropertyToID(nameof(FogOfWarResolution));

			public static int FogOfWarTexture { get; } = Shader.PropertyToID(nameof(FogOfWarTexture));

			public static int FogOfWarProperties { get; } = Shader.PropertyToID(nameof(FogOfWarProperties));

			public static int CellsVisibilityBuffer { get; } = Shader.PropertyToID(nameof(CellsVisibilityBuffer));
		}

		private FogOfWar FogOfWar { get; set; }

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

		public void Initialize()
		{
			FogOfWar = ECSUtils.GetSingleton<FogOfWar>();

			var resolution = new Vector4(FogOfWar.Size.x, FogOfWar.Size.y);
			Shader.SetGlobalVector(ShaderVariables.FogOfWarResolution, resolution);

			AnimationMaterial = new Material(Config.PreProcessShader);
			BlurMaterial = new Material(Config.BlurShader);

			AnimationRT = CreateAnimationRT("FogOfWar.Animation");
			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			PropertiesBuffer = new ConstantComputeBuffer<FogOfWarProperties>(ShaderVariables.FogOfWarProperties);

			CellsVisibilityBuffer = new ComputeBuffer(FogOfWar.Visibility.Length, sizeof(CellVisibilityState));

			if (FogOfWar.Explored)
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
				return new RenderTexture(FogOfWar.Size.x, FogOfWar.Size.y,
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
				return new RenderTexture(FogOfWar.Size.x, FogOfWar.Size.y,
					GraphicsFormat.R16G16_SFloat,
					GraphicsFormat.None)
				{
					name = textureName,
					anisoLevel = 0
				};
			}
		}

		protected override FogOfWarPass Setup(FogOfWarRendererConfig config) => new FogOfWarPass(this);

		protected override bool IsInactive() => false;

		public void Dispose()
		{
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

			commandBuffer.SetBufferData(CellsVisibilityBuffer, FogOfWar.Visibility);
			commandBuffer.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			Blitter.BlitTexture(commandBuffer, AnimationRT, AnimationRT, AnimationMaterial, 0);
			Blitter.BlitTexture(commandBuffer, AnimationRT, BlurRT1, BlurMaterial, 0);
			Blitter.BlitTexture(commandBuffer, BlurRT1, BlurRT2, BlurMaterial, 1);
			//commandBuffer.Blit(AnimationRT, AnimationRT, AnimationMaterial, 0);
			//commandBuffer.Blit(AnimationRT, BlurRT1, BlurMaterial, 0);
			//commandBuffer.Blit(BlurRT1, BlurRT2, BlurMaterial, 1);

			commandBuffer.SetGlobalTexture(ShaderVariables.FogOfWarTexture, BlurRT2);
		}
	}
}
