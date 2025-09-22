Shader "Omniverse/Minimap"
{
    Properties
    {
        FogOfWarColor("Fog Of War Color", Color) = (0, 0, 0, 0.5)

        FrustrumColor("Frustrum Color", Color) = (1, 1, 1, 1)
        FrustrumEdgeWidth("Frustrum Edge Width", Float) = 0.01
        FrustrumEdgePower("Frustrum Edge Power", Float) = 1
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
        ENDHLSL

        Pass
        {
            Name "FogOfWar"

            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            float4 FogOfWarColor;

            uniform sampler2D FogOfWarTexture;

            float4 Frag(Varyings input) : SV_Target
            {
                float4 color = (tex2D(FogOfWarTexture, input.texcoord).r + 1) * FogOfWarColor;
                return color;
            }
            ENDHLSL
        }

        Pass
        {
            Name "Frustrum"

            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            #include "Assets/Scripts/Map/Map.hlsl"

            float4 FrustrumColor;
            float FrustrumEdgeWidth;
            float FrustrumEdgePower;

            float4 Point1;
            float4 Point2;
            float4 Point3;
            float4 Point4;

            void CalculateValuesForEdge(float2 a, float2 b, float2 c, out float dist, out float factor)
            {
                float2 normal = normalize(float2(-(b.y - a.y), (b.x - a.x)));
                float distAB = dot(normal, a);
                dist = dot(normal, c) - distAB;
                factor = 1 - saturate(abs(dist) / FrustrumEdgeWidth);
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float aDist, aFactor;
                CalculateValuesForEdge(Point1, Point2, input.texcoord, aDist, aFactor);

                float bDist, bFactor;
                CalculateValuesForEdge(Point2, Point3, input.texcoord, bDist, bFactor);

                float cDist, cFactor;
                CalculateValuesForEdge(Point3, Point4, input.texcoord, cDist, cFactor);

                float dDist, dFactor;
                CalculateValuesForEdge(Point4, Point1, input.texcoord, dDist, dFactor);

                float h = max(aFactor, cFactor);
                float v = max(bFactor, dFactor);
                float sum = lerp(h, 1, v);

                float hh = lerp(sum, h, aDist >= 0 || cDist >= 0);
                float vv = lerp(sum, v, bDist >= 0 || dDist >= 0);

                float opacity = pow(min(hh, vv), FrustrumEdgePower);

                return FrustrumColor * opacity;
            }
            ENDHLSL
        }
    }
}
