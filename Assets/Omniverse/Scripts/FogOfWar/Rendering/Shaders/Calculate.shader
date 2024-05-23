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

    float4 Init(Varyings input) : SV_Target
    {
        const int cellIndex = input.positionCS.x * 128 - (64 - input.positionCS.y);
        const float v = CellsVisibilityBuffer[cellIndex] == CELL_VISIBILITY_CONCEALED;
        return float4(0,0,0, v);
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
        Blend One Zero

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Init
            ENDHLSL
        }
    }
}