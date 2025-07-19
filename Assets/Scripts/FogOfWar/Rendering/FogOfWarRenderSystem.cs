using System;
using System.Linq;
using Dreambox.Rendering.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class FogOfWarRenderSystem : SystemBase
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

		private FogOfWarRenderSettings Settings { get; set; }

		private FogOfWarPass Pass { get; set; }

		private RenderTexture AnimationRT { get; set; }

		private RenderTexture BlurRT1 { get; set; }

		public RenderTexture BlurRT2 { get; set; }

		private ComputeBuffer CellsVisibilityBuffer { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<MapSettings>();
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			RenderPipelineManager.beginContextRendering += OnBeginContextRendering;
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;

			var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();
			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.FogOfWar;

			foreach (FogOfWarMode mode in Enum.GetValues(typeof(FogOfWarMode)).Cast<FogOfWarMode>())
			{
				if (mode == fogOfWarSettings.Mode)
				{
					Shader.EnableKeyword(ShaderVariables.ModeToKeyword(mode));
				}
				else
				{
					Shader.DisableKeyword(ShaderVariables.ModeToKeyword(mode));
				}
			}

			Pass = new FogOfWarPass(Settings.Material)
			{
				renderPassEvent = Settings.RenderPassEvent
			};

			if (fogOfWarSettings.Mode is FogOfWarMode.Revealed)
			{
				return;
			}

			var fogOfWar = SystemAPI.GetSingleton<FogOfWar>();

			var resolution = new Vector4(fogOfWarSettings.Size.x, fogOfWarSettings.Size.y);
			Shader.SetGlobalVector(ShaderVariables.FogOfWarResolution, resolution);

			AnimationRT = CreateAnimationRT("FogOfWar.Animation");
			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			CellsVisibilityBuffer = new ComputeBuffer(fogOfWar.Visibility.Length, sizeof(CellVisibilityState));

			RenderTexture CreateAnimationRT(string textureName)
			{
				return new RenderTexture(fogOfWarSettings.Size.x, fogOfWarSettings.Size.y,
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
				return new RenderTexture(fogOfWarSettings.Size.x, fogOfWarSettings.Size.y,
					GraphicsFormat.R16G16_SFloat,
					GraphicsFormat.None)
				{
					name = textureName,
					anisoLevel = 0
				};
			}
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;

			var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

			if (fogOfWarSettings.Mode is not FogOfWarMode.Revealed)
			{
				AnimationRT.Release();
				BlurRT1.Release();
				BlurRT2.Release();
				CellsVisibilityBuffer.Release();
			}
		}

		private void OnBeginContextRendering(ScriptableRenderContext context, System.Collections.Generic.List<Camera> cameras)
		{
			var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

			if (fogOfWarSettings.Mode is FogOfWarMode.Revealed)
			{
				return;
			}

			var fogOfWar = SystemAPI.GetSingleton<FogOfWar>();

			using var scope = new CommandBufferScope("FogOfWar.PreProcess");
			CommandBuffer commandBuffer = scope.CommandBuffer;

			commandBuffer.SetBufferData(CellsVisibilityBuffer, fogOfWar.Visibility);
			commandBuffer.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			Blitter.BlitTexture(commandBuffer, AnimationRT, AnimationRT, Settings.Material, ShaderPass.Animate);
			Blitter.BlitTexture(commandBuffer, AnimationRT, BlurRT1, Settings.BlurMaterial, 0);
			Blitter.BlitTexture(commandBuffer, BlurRT1, BlurRT2, Settings.BlurMaterial, 1);

			commandBuffer.SetGlobalTexture(ShaderVariables.FogOfWarTexture, BlurRT2);
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (Settings.CameraType.HasFlag(camera.cameraType))
			{
				camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(Pass);
			}
		}

		protected override void OnUpdate()
		{
		}
	}
}
