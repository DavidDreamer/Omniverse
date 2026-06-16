Shader "Omniverse/FogOfWar"
{
    Properties
    {
        BaseColor ("Base Color", Color) = (0, 0, 0, 1)
        ExploredColor ("Explored Color", Color) = (0.5, 0.5, 0.5, 1)

        CloudsTexture1 ("Clouds 1", 2D) = "white"
        CloudsTexture2 ("Clouds 2", 2D) = "white"

        AnimationSpeed ("Animation Speed", Float) = 1
        BorderLength ("Border Length", Float) = 0.01
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        uniform float4 FogOfWarResolution;
        TEXTURE2D_X(FogOfWarTexture);
        SAMPLER(sampler_FogOfWarTexture);
        uniform StructuredBuffer<int> CellsVisibilityBuffer;

        #define CELL_VISIBILITY_UNEXPLORED 0
        #define CELL_VISIBILITY_EXPLORED 1
        #define CELL_VISIBILITY_VISIBLE 2
        ENDHLSL

        Pass
        {
            Name "Animate"

            ZTest Always
            ZWrite Off
            Cull Off
            Blend One One

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            float AnimationSpeed;

            float4 Frag(Varyings input) : SV_Target
            {
                int cellIndex = input.positionCS.x * FogOfWarResolution.x - (FogOfWarResolution.x / 2 - input.positionCS.y);
                int currentCellState = CellsVisibilityBuffer[cellIndex];
                float speed = AnimationSpeed * unity_DeltaTime.x;
                float delta = (currentCellState == CELL_VISIBILITY_VISIBLE ? -1 : 1) * speed;
                float unexplored = currentCellState == CELL_VISIBILITY_UNEXPLORED;
                return float4(delta, unexplored, 0, 0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Apply"

            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fragment MODE_REVEALED MODE_EXPLORED MODE_UNEXPLORED
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Assets/Scripts/Map/Map.hlsl"
            
            float4 BaseColor;
            float4 ExploredColor;

            sampler2D CloudsTexture1;
            float4 CloudsTexture1_ST;

            sampler2D CloudsTexture2;
            float4 CloudsTexture2_ST;

            float BorderLength;

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

                float2 cloudsUV1 = uv * CloudsTexture1_ST.xy + CloudsTexture1_ST.zw * _Time.y;
                float2 cloudsUV2 = uv * CloudsTexture2_ST.xy + CloudsTexture1_ST.zw * _Time.y;

                float cloud1 = tex2D(CloudsTexture1, cloudsUV1).r;
                float cloud2 = tex2D(CloudsTexture2, cloudsUV2).r;

                float cloud = lerp(cloud1, cloud2, 0.5);

                float2 distanceFromBorder = 1 - (abs(float2(0.5, 0.5) - uv) * 2);
                float2 distanceFromBorderClamped = 1 - saturate(distanceFromBorder / BorderLength);
                float4 outOfBorderShading = lerp(distanceFromBorderClamped.x, 1, distanceFromBorderClamped.y) * BaseColor;
                outOfBorderShading = saturate(outOfBorderShading + cloud * outOfBorderShading);

                #ifdef MODE_REVEALED
                    return outOfBorderShading;
                #else
                    float4 fowData = SAMPLE_TEXTURE2D_X(FogOfWarTexture, sampler_FogOfWarTexture, uv);
                    float fogValue = (fowData.r + 1.0) * 0.5;

                    fogValue = saturate(fogValue + cloud * fogValue);
                
                    #ifdef MODE_EXPLORED
                        float4 fogColor = ExploredColor * fogValue;
                    #else
                        float4 fogColor = lerp(ExploredColor, BaseColor, fowData.g) * fogValue;
                    #endif
                
                    return max(fogColor, outOfBorderShading);
                #endif
            }
            ENDHLSL
        }
    }
}
