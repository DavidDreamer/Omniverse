using System;
using System.Linq;
using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class FogOfWarRenderer : CustomRenderer<FogOfWarRendererConfig, FogOfWarPass>
	{
		public static class ShaderPass
		{
			public const int Animate = 0;
			public const int Apply = 1;
		}

		private static class ShaderVariables
		{
			public static string ModeToKeyword(FogOfWarMode mode) => $"MODE_{mode.ToString().ToUpper()}";

			public static int FogOfWarResolution { get; } = Shader.PropertyToID(nameof(FogOfWarResolution));

			public static int FogOfWarTexture { get; } = Shader.PropertyToID(nameof(FogOfWarTexture));

			public static int CellsVisibilityBuffer { get; } = Shader.PropertyToID(nameof(CellsVisibilityBuffer));
		}

		public FogOfWar FogOfWar { get; private set; }

		private RenderTexture AnimationRT { get; set; }

		public RenderTexture BlurRT1 { get; set; }

		public RenderTexture BlurRT2 { get; set; }
		
		private ComputeBuffer CellsVisibilityBuffer { get; set; }

		public void Start()
		{
			GameOptions gameOptions = ECSUtils.GetSingletonManaged<GameOptions>();

			foreach (FogOfWarMode mode in Enum.GetValues(typeof(FogOfWarMode)).Cast<FogOfWarMode>())
			{
				if (mode == gameOptions.FogOfWarMode)
				{
					Config.Material.EnableKeyword(ShaderVariables.ModeToKeyword(mode));
				}
				else
				{
					Config.Material.DisableKeyword(ShaderVariables.ModeToKeyword(mode));
				}
			}

			if (gameOptions.FogOfWarMode is FogOfWarMode.Revealed)
			{
				return;
			}

			FogOfWar = ECSUtils.GetSingleton<FogOfWar>();

			var resolution = new Vector4(FogOfWar.Size.x, FogOfWar.Size.y);
			Shader.SetGlobalVector(ShaderVariables.FogOfWarResolution, resolution);

			AnimationRT = CreateAnimationRT("FogOfWar.Animation");
			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			CellsVisibilityBuffer = new ComputeBuffer(FogOfWar.Visibility.Length, sizeof(CellVisibilityState));

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

		protected override FogOfWarPass Setup() => new FogOfWarPass(this);

		protected override bool IsInactive() => false;

		public void OnDestroy()
		{
			AnimationRT?.Release();
			BlurRT1?.Release();
			BlurRT2?.Release();

			CellsVisibilityBuffer?.Release();
		}

		private void LateUpdate()
		{
			GameOptions gameOptions = ECSUtils.GetSingletonManaged<GameOptions>();
			if (gameOptions.FogOfWarMode is FogOfWarMode.Revealed)
			{
				return;
			}

			using var scope = new CommandBufferScope("FogOfWar.PreProcess");
			CommandBuffer commandBuffer = scope.CommandBuffer;

			commandBuffer.SetBufferData(CellsVisibilityBuffer, FogOfWar.Visibility);
			commandBuffer.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			Blitter.BlitTexture(commandBuffer, AnimationRT, AnimationRT, Config.Material, ShaderPass.Animate);
			Blitter.BlitTexture(commandBuffer, AnimationRT, BlurRT1, Config.BlurMaterial, 0);
			Blitter.BlitTexture(commandBuffer, BlurRT1, BlurRT2, Config.BlurMaterial, 1);

			commandBuffer.SetGlobalTexture(ShaderVariables.FogOfWarTexture, BlurRT2);
		}
	}
}
