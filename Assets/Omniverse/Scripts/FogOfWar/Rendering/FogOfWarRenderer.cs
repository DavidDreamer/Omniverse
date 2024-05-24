using System;
using System.Runtime.InteropServices;
using System.Threading;
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
		private static class ShaderVariables
		{
			public static int FogOfWarTexture { get; } = Shader.PropertyToID(nameof(FogOfWarTexture));
			public static int FogOfWarProperties { get; } = Shader.PropertyToID(nameof(FogOfWarProperties));
			public static int CellsVisibilityBuffer { get; } = Shader.PropertyToID(nameof(CellsVisibilityBuffer));
		}
		
		[field: SerializeField]
		private Shaders Shaders { get; set; }
		
		[field: SerializeField]
		private FogOfWarProperties Properties { get; set; }
		
		public RenderTexture AnimationTexture1;
		public RenderTexture AnimationTexture2;
		
		public RenderTexture RenderTexture;
		public RenderTexture RenderTexture2;

		private Material CalcualteMaterial;
		private Material BlurMaterial;

		private ComputeBuffer CellsVisibilityBuffer { get; set; }
		
		public float Radius;

		public bool ApplyBlur;

		[Inject]
		public Manager FogOfWar { get; set; }

		private RenderPass RenderPass { get; set; }
		
		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				UpdateGlobalShaderVariables();
			}
		}
		
		private void OnEnable()
		{
			RenderPass = new RenderPass(Shaders)
			{
				renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
			};
		
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		private void OnDisable()
		{
			RenderPass.Dispose();
			
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void UpdateGlobalShaderVariables() =>
			ConstantBuffer.PushGlobal(Properties, ShaderVariables.FogOfWarProperties);

		private void OnBeginCameraRendering(ScriptableRenderContext context, UnityEngine.Camera cam)
		{
			if (cam.cameraType is CameraType.Game or CameraType.SceneView)
			{
				cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(RenderPass);
			}
		}

		public void Initialize()
		{
			AnimationTexture1 = CreateAnimationRenderTexture("FogOfWar.Animation.1");
			AnimationTexture2 = CreateAnimationRenderTexture("FogOfWar.Animation.2");
			
			RenderTexture = CreateRenderTexture("FogOfWar0");
			RenderTexture2 = CreateRenderTexture("FogOfWar1");
	
			BlurMaterial = new Material(Shaders.Blur);
			BlurMaterial.SetFloat("Factor", 1);
			BlurMaterial.SetKeyword(new LocalKeyword(BlurMaterial.shader, "ALGORITHM_GAUSSIAN"), true);

			CalcualteMaterial = new Material(Shaders.Calculate);
			CellsVisibilityBuffer = new ComputeBuffer(FogOfWar.CellsVisibilityPerFaction[0].Length, sizeof(CellVisibilityState));
			
			RenderTexture CreateAnimationRenderTexture(string textureName)
			{
				return new RenderTexture(FogOfWar.Resolution.x, FogOfWar.Resolution.y,
					GraphicsFormat.R16G16B16A16_SFloat,
					GraphicsFormat.None)
				{
					name = textureName,
					filterMode = FilterMode.Point,
					anisoLevel = 0
				};
			}
			
			RenderTexture CreateRenderTexture(string textureName)
			{
				return new RenderTexture(FogOfWar.Resolution.x, FogOfWar.Resolution.y,
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
			CellsVisibilityBuffer.Release();
			
			RenderTexture.Release();
			RenderTexture2.Release();
		}

		private int abbi = 0;
		
		public void LateUpdate()
		{
			BlurMaterial.SetFloat("Radius", Radius);

			CellsVisibilityBuffer.SetData(FogOfWar.CellsVisibilityPerFaction[0]);
			Shader.SetGlobalBuffer(ShaderVariables.CellsVisibilityBuffer, CellsVisibilityBuffer);

			bool it = abbi % 2 == 0;

			RenderTexture source = it ? AnimationTexture1 : AnimationTexture2;
			RenderTexture target = it ? AnimationTexture2 : AnimationTexture1;

			CalcualteMaterial.SetTexture("_MainTex", source);
			CalcualteMaterial.SetVector("_BlitScaleBias", new Vector4(1,1,0,0));
			Graphics.Blit(source, target, CalcualteMaterial);

			abbi++;
			
			if (ApplyBlur)
			{
				Graphics.Blit(target, RenderTexture, BlurMaterial, 0);
				Graphics.Blit(RenderTexture, RenderTexture2, BlurMaterial, 1);
			}

			Shader.SetGlobalTexture(ShaderVariables.FogOfWarTexture, RenderTexture2);
		}
	}
}
