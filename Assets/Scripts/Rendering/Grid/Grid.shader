Shader "Omniverse/Grid"
{
	Properties
	{
		GridColor ("Grid Color", Color) = (1,1,1,1)
		RenderingLayer ("Rendering Layer", Integer) = 2
		Thickness ("Thickness", Range(0, 1)) = 0.05

		FreeColor ("Free Color", Color) = (1,1,1,1)
		ObstacleColor ("Obstacle Color", Color) = (1,1,1,1)

		HighPassabilityColor ("High Passability Color", Color) = (1,1,1,1)
		LowPassabilityColor ("Low Passability Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Cull Off 
		ZWrite Off 
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareRenderingLayerTexture.hlsl"

		float4 GridColor;
		uint RenderingLayer;
		float Thickness;

		void CullByLayer(float4 positionCS)
		{
			#ifdef _RENDER_PASS_ENABLED
				uint surfaceRenderingLayer = DecodeMeshRenderingLayer(LOAD_FRAMEBUFFER_X_INPUT(GBUFFER4, positionCS.xy).r);
			#else
				uint surfaceRenderingLayer = LoadSceneRenderingLayer(positionCS.xy);
			#endif

			clip((surfaceRenderingLayer & RenderingLayer) - 0.1);
		}

		float3 GetWorldSpacePosition(float4 positionCS)
		{
			float2 UV = positionCS.xy / _ScaledScreenParams.xy;

			// Sample the depth from the Camera depth texture.
			#if UNITY_REVERSED_Z
			real depth = SampleSceneDepth(UV);
			#else
			// Adjust Z to match NDC for OpenGL ([-1, 1])
			real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
			#endif
				
			float3 position = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);

			return position;
		}

		int GetCellId(float3 positionWS)
		{
			int x = (int)floor(positionWS.x) + 128;
			int y = (int)floor(positionWS.z) + 128;
			return y * 256 + x;;
		}

		float GetGridFactor(float3 positionWS)
		{
			float2 worldPosFrac = frac(positionWS.xz);
			float2 aa = saturate(worldPosFrac / Thickness);
			float2 bb = saturate((1 - worldPosFrac) / Thickness);

			float h = 1 - min(aa.x, bb.x);
			float v = 1 - min(aa.y, bb.y);

			return lerp(h, 1, v);
		}
		ENDHLSL

		Pass
		{
			Name "Obstacles"
		
			HLSLPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag

			float4 FreeColor;
			float4 ObstacleColor;

			StructuredBuffer<int> ObstaclesBuffer;

			float4 Frag(Varyings input) : SV_Target
			{
				CullByLayer(input.positionCS);

				float3 positionWS = GetWorldSpacePosition(input.positionCS);
				int cellId = GetCellId(positionWS);
				float gridFactor = GetGridFactor(positionWS);
 
				bool isObstacle = ObstaclesBuffer[cellId];

				float4 cellColor = isObstacle ? ObstacleColor : FreeColor;

				float4 finalColor = lerp(cellColor, GridColor, gridFactor);

				return finalColor;
			}
			ENDHLSL
		}

		 Pass
		{
			Name "Passability"

			HLSLPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag

			float4 HighPassabilityColor;
			float4 LowPassabilityColor;

			StructuredBuffer<float> PenaltyBuffer;

			float4 Frag(Varyings input) : SV_Target
			{
				CullByLayer(input.positionCS);

				float3 positionWS = GetWorldSpacePosition(input.positionCS);
				int cellId = GetCellId(positionWS);
				float gridFactor = GetGridFactor(positionWS);

				float penalty = PenaltyBuffer[cellId];
				float4 cellColor = lerp(HighPassabilityColor, LowPassabilityColor, penalty - 1);

				float4 finalColor = lerp(cellColor, GridColor, gridFactor);

				return finalColor;
			}
			ENDHLSL
		}
	}
}
