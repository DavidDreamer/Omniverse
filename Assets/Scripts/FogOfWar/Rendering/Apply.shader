Shader "Hidden/Omniverse/FogOfWar/Apply"
{
    Properties
    {
        _NoiseTexture ("Noise", 2D) = "white"
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        ZTest Always ZWrite Off Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Apply"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fragment _ FOG_OF_WAR_EXPLORED
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Assets/Scripts/Mapping/Map.hlsl"
            #include "Common.hlsl"
            
            sampler2D _NoiseTexture;

            float4 Frag(Varyings input) : SV_Target
            {
                float2 UV = input.positionCS.xy / _ScaledScreenParams.xy;

                // Sample the depth from the Camera depth texture.
                #if UNITY_REVERSED_Z
                real depth = SampleSceneDepth(UV);
                #else
                // Adjust Z to match NDC for OpenGL ([-1, 1])
                real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
                #endif
                
                float3 worldPos = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);
                float2 uv = saturate((worldPos.xz + MapSize.xy / 2) / MapSize.xy);

                float2 tiling1 = float2(10, 10);
                float2 tiling2 = float2(1, 1);

                float2 offset = float2(0.5 * _Time.y, 0.5 * _Time.y);

                float speed1 = 0.05;
                float speed2 = 0.1;

                float2 cloudsUV1 = uv * tiling1 + offset * speed1;
                float2 cloudsUV2 = uv * tiling2 - offset * speed2;

                float cloud1 = tex2D(_NoiseTexture, cloudsUV1).r;
                float cloud2 = tex2D(_NoiseTexture, cloudsUV2).r;

                float cloud = lerp(cloud1, cloud2, 0.5);

                float2 distanceFromBorder = 1 - (abs(float2(0.5, 0.5) - uv) * 2);
                float2 distanceFromBorderClamped = 1 - saturate(distanceFromBorder / FogOfWarBorderLength);
                float4 outOfBorderShading = max(distanceFromBorderClamped.x, distanceFromBorderClamped.y) * FogOfWarUnexploredColor;
                outOfBorderShading = saturate(outOfBorderShading + cloud * outOfBorderShading);

                float4 fowData = tex2D(FogOfWarTexture, uv);
                float fogValue = (fowData.r + 1.0) * 0.5;

                fogValue = saturate(fogValue + cloud * fogValue);
                
                #ifdef FOG_OF_WAR_EXPLORED
                float4 fogColor = FogOfWarExploredColor * fogValue;
                #else
                float4 fogColor = lerp(FogOfWarExploredColor, FogOfWarUnexploredColor, fowData.g) * fogValue;
                #endif
                
                return max(fogColor, outOfBorderShading);
            }
            ENDHLSL
        }
    }
}
