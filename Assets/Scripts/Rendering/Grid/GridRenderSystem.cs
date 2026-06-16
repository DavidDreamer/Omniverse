using System.Linq;
using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	[DisableAutoCreation]
	public partial class GridRenderSystem : SystemBase
	{
		private static class ShaderVariables
		{
			public static int ObstaclesBuffer { get; } = Shader.PropertyToID(nameof(ObstaclesBuffer));
			public static int PenaltyBuffer { get; } = Shader.PropertyToID(nameof(PenaltyBuffer));
		}

		private GridRenderSettings Settings { get; set; }

		private GridRenderPass Pass { get; set; }

		private ComputeBuffer ObstaclesBuffer { get; set; }

		private ComputeBuffer PenaltyBuffer { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<RenderSettings>();
			RequireForUpdate<Map>();
		}

		protected override void OnStartRunning()
		{
			var map = SystemAPI.GetSingleton<Map>();
			ObstaclesBuffer = new ComputeBuffer(map.Nodes.Length, Marshal.SizeOf<int>());
			Shader.SetGlobalBuffer(ShaderVariables.ObstaclesBuffer, ObstaclesBuffer);

			PenaltyBuffer = new ComputeBuffer(map.Penalties.Length, Marshal.SizeOf<float>());
			Shader.SetGlobalBuffer(ShaderVariables.PenaltyBuffer, PenaltyBuffer);

			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Grid;

			Pass = new(Settings, EntityManager)
			{
				renderPassEvent = Settings.RenderPassEvent
			};

			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		protected override void OnStopRunning()
		{
			ObstaclesBuffer.Dispose();
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
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
			var map = SystemAPI.GetSingleton<Map>();
			ObstaclesBuffer.SetData(map.Obstacles.Select(x => x ? 1 : 0).ToArray());
			PenaltyBuffer.SetData(map.Penalties);
		}
	}
}
