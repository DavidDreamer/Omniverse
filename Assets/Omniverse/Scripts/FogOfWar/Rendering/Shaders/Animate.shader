Shader "Hidden/Omniverse/FogOfWar/Animate"
{
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

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // the input structure (Attributes), and the output structure (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Assets/Omniverse/Scripts/Mapping/Map.hlsl"
            #include "Assets/Omniverse/Scripts/FogOfWar/Rendering/Shaders/Properties.hlsl"

            sampler2D _MainTex;
            
            float4 Init(Varyings input) : SV_Target
            {
                //const float previousValue = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord).a;
                const float previousValue = tex2D(_MainTex, input.texcoord).r;
                const int cellIndex = input.positionCS.x * 128 - (64 - input.positionCS.y);
                const int currentCellState = CellsVisibilityBuffer[cellIndex];
                const float delta = (currentCellState == CELL_VISIBILITY_VISIBLE ? 1 : -1) * FogOfWarAnimationSpeed * unity_DeltaTime.x;
                const float unexplored = currentCellState == CELL_VISIBILITY_UNEXPLORED;
                const float newValue = saturate(previousValue + delta);
                return float4(newValue, unexplored, 0, 0);
            }
            ENDHLSL
        }
    }
}