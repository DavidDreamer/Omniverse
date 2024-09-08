using System;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class AbilityRendererPass : ScriptableRenderPass, IDisposable
	{
		private AbilityRenderer Renderer { get; }

		public AbilityRendererPass(AbilityRenderer renderer)
		{
			Renderer = renderer;
		}

		public void Dispose()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "Ability");
			var commandBuffer = scope.CommandBuffer;

			AbilityRendererConfig config = Renderer.Config;
			AbilityController abilityController = Renderer.AbilityController;

			var matrix = abilityController.ActiveUnit.transform.localToWorldMatrix * Matrix4x4.Scale(Vector3.one * abilityController.ActiveAbility.Desc.Target.Range);
			var drawMeshParams = config.Range;
			commandBuffer.DrawMesh(
				drawMeshParams.Mesh,
				matrix,
				drawMeshParams.Material,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.ShaderPass);
	
		}
	}
}
