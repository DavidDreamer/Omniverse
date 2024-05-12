Shader "Hidden/Omniverse/FogOfWar"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    // The Blit.hlsl file provides the vertex shader (Vert),
    // the input structure (Attributes), and the output structure (Varyings)
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

    CBUFFER_START(FogOfWarProperties)
    float4 FogOfWarColor;
    CBUFFER_END
    
    uniform sampler2D FogOfWarTexture;

    float4 ApplyFogOfWar(Varyings input) : SV_Target
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

        const float2 uv = worldPos.xz / 256;
        
        const float s = tex2D(FogOfWarTexture, uv).a;
        
        return FogOfWarColor * s;
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
            #pragma fragment ApplyFogOfWar
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