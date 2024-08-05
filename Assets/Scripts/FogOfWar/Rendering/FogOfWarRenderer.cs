using System;
using Dreambox.Rendering.Core;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.FogOfWar.Rendering
{
	public class FogOfWarRenderer : MonoBehaviour, IInitializable, IDisposable
	{
		[field: SerializeField]
		private Shaders Shaders { get; set; }

		[field: SerializeField]
		private Properties Properties { get; set; }

		[field: SerializeField]
		private BlurSettings BlurSettings { get; set; }

		[Inject]
		public Manager Manager { get; set; }

		private Material AnimationMaterial { get; set; }

		private Material BlurMaterial { get; set; }

		private RenderTexture AnimationRT { get; set; }

		public RenderTexture BlurRT1 { get; set; }

		public RenderTexture BlurRT2 { get; set; }

		private ComputeBuffer PropertiesBuffer { get; set; }

		private Properties[] PropertiesData { get; set; }

		private ComputeBuffer CellsVisibilityBuffer { get; set; }

		private ApplyPass ApplyPass { get; set; }

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				UpdateShaderVariables();
			}
		}

		private void UpdateShaderVariables()
		{
			var resolution = new Vector4(Manager.Resolution.x, Manager.Resolution.y);
			Shader.SetGlobalVector(ShaderVariables.FogOfWarResolution, resolution);

			if (PropertiesBuffer != null)
			{
				PropertiesData[0] = Properties;
				PropertiesBuffer.SetData(PropertiesData);
			}

			if (BlurMaterial != null)
			{
				BlurSettings.ApplyTo(BlurMaterial);
			}
		}

		public void Initialize()
		{
			AnimationMaterial = new Material(Shaders.PreProcess);
			BlurMaterial = new Material(Shaders.Blur);

			AnimationRT = CreateAnimationRT("FogOfWar.Animation");
			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			PropertiesBuffer = new ComputeBuffer(1, UnsafeUtility.SizeOf<Properties>(), ComputeBufferType.Constant);
			PropertiesData = new Properties[1];
			Shader.SetGlobalConstantBuffer(ShaderVariables.FogOfWarProperties, PropertiesBuffer, 0, PropertiesBuffer.stride);

			CellsVisibilityBuffer =
				new ComputeBuffer(Manager.CellsVisibilityPerFaction[0].Length, sizeof(CellVisibilityState));

			ApplyPass = new ApplyPass(Manager, Shaders.Apply)
			{
				renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
			};

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

		public void Dispose()
		{
			CoreUtils.Destroy(AnimationMaterial);
			CoreUtils.Destroy(BlurMaterial);

			AnimationRT.Release();
			BlurRT1.Release();
			BlurRT2.Release();

			PropertiesBuffer.Release();
			CellsVisibilityBuffer.Release();

			ApplyPass.Dispose();
		}

		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (cam.cameraType is CameraType.Game or CameraType.SceneView)
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(ApplyPass);
			}
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
