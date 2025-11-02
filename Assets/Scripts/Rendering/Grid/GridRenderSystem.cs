using System.Linq;
using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class GridRenderSystem : SystemBase
	{
		private static class ShaderVariables
		{
			public static int ObstaclesBuffer { get; } = Shader.PropertyToID(nameof(ObstaclesBuffer));
			public static int PassabilityBuffer { get; } = Shader.PropertyToID(nameof(PassabilityBuffer));
		}

		private GridRenderSettings Settings { get; set; }

		private GridRenderPass Pass { get; set; }

		private ComputeBuffer ObstaclesBuffer { get; set; }

		private ComputeBuffer PassabilityBuffer { get; set; }

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

			PassabilityBuffer = new ComputeBuffer(map.Passability.Length, Marshal.SizeOf<float>());
			Shader.SetGlobalBuffer(ShaderVariables.PassabilityBuffer, PassabilityBuffer);

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
			PassabilityBuffer.SetData(map.Passability);
		}
	}
}
