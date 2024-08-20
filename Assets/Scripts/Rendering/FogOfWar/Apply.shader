Shader "Hidden/Omniverse/FogOfWar/Apply"
{
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
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fragment _ FOG_OF_WAR_EXPLORED
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Assets/Scripts/Mapping/Map.hlsl"
            #include "Common.hlsl"
            
            float4 Frag(Varyings input) : SV_Target
            {
                const float2 UV = input.positionCS.xy / _ScaledScreenParams.xy;

                // Sample the depth from the Camera depth texture.
                #if UNITY_REVERSED_Z
                real depth = SampleSceneDepth(UV);
                #else
                // Adjust Z to match NDC for OpenGL ([-1, 1])
                real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
                #endif
                
                const float3 worldPos = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);
                const float2 uv = worldPos.xz / MapSize.xy;

                const float4 fowData = tex2D(FogOfWarTexture, uv);

                const bool outOfBounds = uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1;

                const float v = (fowData.r + 1.0) * 0.5;
                
                const float animationValue = outOfBounds ? 1 :  1 - v;
                
                #ifdef FOG_OF_WAR_EXPLORED
                return FogOfWarExploredColor * animationValue;
                #else
                return lerp(FogOfWarExploredColor, FogOfWarUnexploredColor, fowData.g) * animationValue;
                #endif
            }
            ENDHLSL
        }
    }
}
