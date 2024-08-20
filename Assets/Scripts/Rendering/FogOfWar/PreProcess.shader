Shader "Hidden/Omniverse/FogOfWar/PreProcess"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

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

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Common.hlsl"

            sampler2D _MainTex;

            float4 Frag(Varyings input) : SV_Target
            {
                const int cellIndex = input.positionCS.x * FogOfWarResolution.x - (FogOfWarResolution.x / 2 - input.positionCS.y);
                const int currentCellState = CellsVisibilityBuffer[cellIndex];
                const float speed = FogOfWarAnimationSpeed * unity_DeltaTime.x;
                const float delta = (currentCellState == CELL_VISIBILITY_VISIBLE ? 1 : -1) * speed;
                const float unexplored = currentCellState == CELL_VISIBILITY_UNEXPLORED;
                return float4(delta, unexplored, 0, 0);
            }
            ENDHLSL
        }
    }
}