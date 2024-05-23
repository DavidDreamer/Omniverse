Shader "Hidden/Omniverse/FogOfWar"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    // The Blit.hlsl file provides the vertex shader (Vert),
    // the input structure (Attributes), and the output structure (Varyings)
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
    #include "Assets/Omniverse/Scripts/Mapping/Map.hlsl"
    #include "Assets/Omniverse/Scripts/FogOfWar/Rendering/Shaders/Common.hlsl"
    
     // note: no SV_POSITION in this struct
    struct v2f {
        float2 uv : TEXCOORD0;
    };

    v2f vert (
        float4 vertex : POSITION, // vertex position input
        float2 uv : TEXCOORD0, // texture coordinate input
        out float4 outpos : SV_POSITION // clip space position output
        )
    {
        v2f o;
        o.uv = uv;
        outpos = TransformObjectToHClip(vertex);
        return o;
    }

    float4 Init(v2f input, float4 screenPos : VPOS) : SV_Target
    {
        const int cellIndex = screenPos.x * 128 + screenPos.y;
        const float delta = CellsVisibilityBuffer[cellIndex] == CELL_VISIBILITY_VISIBLE ? -0.01 : 0.01;
        float3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.uv).rgb;
        color = saturate(color + delta);
        return float4(color.rgb, 1);
    }
    
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

        const int2 uv = worldPos.xz / MapSize.xy;

        const int x = (int)(worldPos.x / 2);
        const int y = (int)(worldPos.z / 2);
        
        const int cellv = CellsVisibilityBuffer[x * 128 + y];
        const float s = cellv == CELL_VISIBILITY_CONCEALED;
        
        return FogOfWarConcealedColor * s;
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
            #pragma multi_compile_fragment MODE_LIGHT MODE_HARD
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
        
//         Pass
//        {
//            Name "Init"
//
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment Init
//            ENDHLSL
//        }
    }
}