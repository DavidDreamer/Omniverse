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
            #pragma fragment Init

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Assets/Omniverse/Scripts/Mapping/Map.hlsl"
            #include "Assets/Omniverse/Scripts/FogOfWar/Rendering/Shaders/Properties.hlsl"

            sampler2D _MainTex;

            float4 Init(Varyings input) : SV_Target
            {
                const int cellIndex = input.positionCS.x * 128 - (64 - input.positionCS.y);
                const int currentCellState = CellsVisibilityBuffer[cellIndex];
                const float delta = (currentCellState == CELL_VISIBILITY_VISIBLE ? 1 : -1) * FogOfWarAnimationSpeed *
                    unity_DeltaTime.x;
                const float unexplored = currentCellState == CELL_VISIBILITY_UNEXPLORED;
                return float4(delta, unexplored, 0, 0);
            }
            ENDHLSL
        }
    }
}