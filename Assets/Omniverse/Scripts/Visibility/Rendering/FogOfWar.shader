Shader "Hidden/Omniverse/FogOfWar"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    // The Blit.hlsl file provides the vertex shader (Vert),
    // the input structure (Attributes), and the output structure (Varyings)
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

    uniform sampler2D FogOfWarTexture;

    float4 RedTint(Varyings input) : SV_Target
    {
        // To calculate the UV coordinates for sampling the depth buffer,
        // divide the pixel location by the render target resolution
        // _ScaledScreenParams.
        float2 UV = input.positionCS.xy / _ScaledScreenParams.xy;

        // Sample the depth from the Camera depth texture.
        #if UNITY_REVERSED_Z
        real depth = SampleSceneDepth(UV);
        #else
                // Adjust Z to match NDC for OpenGL ([-1, 1])
                real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
        #endif

        // Reconstruct the world space positions.
        float3 worldPos = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);

        const float2 uv = worldPos.xz / 256;

        // float3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord).rgb;

        const float s = tex2D(FogOfWarTexture, uv).a;

        // const float3 cc = lerp(0, color, s);

        return float4(0, 0, 0, s);
    }

    float4 SimpleBlit(Varyings input) : SV_Target
    {
        float3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord).rgb;
        return float4(color.rgb, 1);
    }
    ENDHLSL

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
            Name "RedTint"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment RedTint
            ENDHLSL
        }

        Pass
        {
            Name "SimpleBlit"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment SimpleBlit
            ENDHLSL
        }
    }
}