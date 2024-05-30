using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.FogOfWar.Rendering
{
	public class FogOfWarRenderer: MonoBehaviour, IInitializable, IDisposable
	{
		[field: SerializeField]
		private Shaders Shaders { get; set; }

		[field: SerializeField]
		private Properties Properties { get; set; }

		public float Radius;

		[Inject]
		public Manager Manager { get; set; }

		private Material AnimationMaterial { get; set; }

		private Material BlurMaterial { get; set; }

		private RenderTexture AnimationRT1 { get; set; }

		private RenderTexture AnimationRT2 { get; set; }

		public RenderTexture BlurRT1 { get; set; }

		public RenderTexture BlurRT2 { get; set; }

		private ComputeBuffer CellsVisibilityBuffer { get; set; }

		private bool SwapAnimationBuffers { get; set; }

		private ApplyPass ApplyPass { get; set; }

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				UpdateGlobalShaderVariables();
			}
		}

		public void Initialize()
		{
			AnimationMaterial = new Material(Shaders.Animate);
			BlurMaterial = new Material(Shaders.Blur);

			AnimationRT1 = CreateAnimationRT("FogOfWar.Animation.1");
			AnimationRT2 = CreateAnimationRT("FogOfWar.Animation.2");

			BlurRT1 = CreateBlurRT("FogOfWar.Blur.1");
			BlurRT2 = CreateBlurRT("FogOfWar.Blur.2");

			CellsVisibilityBuffer =
				new ComputeBuffer(Manager.CellsVisibilityPerFaction[0].Length, sizeof(CellVisibilityState));

			ApplyPass = new ApplyPass(Manager, Shaders.Apply)
			{
				renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
			};

			RenderTexture CreateAnimationRT(string textureName)
			{
				return new RenderTexture(Manager.Resolution.x, Manager.Resolution.y,
					GraphicsFormat.R16G16B16A16_SFloat,
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
					GraphicsFormat.R16G16B16A16_SFloat,
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

			AnimationRT1.Release();
			AnimationRT2.Release();

			BlurRT1.Release();
			BlurRT2.Release();

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

		private void UpdateGlobalShaderVariables() =>
			ConstantBuffer.PushGlobal(Properties, ShaderVariables.FogOfWarProperties);

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (cam.cameraType is CameraType.Game or CameraType.SceneView)
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(ApplyPass);
			}
		}

		private void LateUpdate()
		{
			CommandBuffer cmd = CommandBufferPool.Get("FogOfWar.PreProcess");

			CellsVisibilityBuffer.SetData(Manager.CellsVisibilityPerFaction[0]);
			Shader.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			RenderTexture source = SwapAnimationBuffers ? AnimationRT2 : AnimationRT1;
			RenderTexture target = SwapAnimationBuffers ? AnimationRT1 : AnimationRT2;

			AnimationMaterial.SetTexture("_MainTex", source);
			AnimationMaterial.SetVector("_BlitScaleBias", new Vector4(1, 1, 0, 0));
			cmd.Blit(source, target, AnimationMaterial);

			SwapAnimationBuffers = !SwapAnimationBuffers;

			BlurMaterial.SetFloat("Factor", 1);
			BlurMaterial.SetKeyword(new LocalKeyword(BlurMaterial.shader, "ALGORITHM_GAUSSIAN"), true);
			BlurMaterial.SetFloat("Radius", Radius);
			cmd.Blit(target, BlurRT1, BlurMaterial, 0);
			cmd.Blit(BlurRT1, BlurRT2, BlurMaterial, 1);

			cmd.SetGlobalTexture(ShaderVariables.FogOfWarTexture, BlurRT2);

			Graphics.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}
	}
}
