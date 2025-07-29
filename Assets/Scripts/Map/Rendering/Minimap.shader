Shader "Omniverse/Minimap"
{
    Properties
    {
        FogOfWarColor("Fog Of War Color", Color) = (0, 0, 0, 0.5)
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
            
            float4 Point1;
            float4 Point2;
            float4 Point3;
            float4 Point4;

            float4 Frag(Varyings input) : SV_Target
            {
                //TEMP: make valid frustrum diplaying
                float intersectionX = (input.texcoord.y - Point1.y) * (Point2.x - Point1.x) / (Point2.y - Point1.y) + Point1.x;
                bool condd = intersectionX > min(Point1.x, Point2.x) && intersectionX < max(Point1.x, Point2.x);
      
                bool condH = input.texcoord.x >= Point1.x && input.texcoord.x <= Point3.x;
                bool condV = input.texcoord.y >= Point1.y && input.texcoord.y <= Point2.y;
                
                bool hd = max(max(0, input.texcoord.x - Point1.x), max(0, Point3.x - input.texcoord.x));
                bool vd = max(max(0, input.texcoord.y - Point1.y), max(0, Point2.y - input.texcoord.y));
                bool d = min(hd, vd);

                float width = 0.02;

                float distanceX = input.texcoord.x - Point1.x;
                bool condX = distanceX >= 0 && distanceX <= width;

                float distanceX2 = Point3.x - input.texcoord.x;
                bool condX2 = distanceX2 >= 0 && distanceX2 <= width;

                float distanceY = input.texcoord.y - Point1.y;
                bool condY = distanceY >= 0 && distanceY <= width;

                float distanceY2 = Point2.y - input.texcoord.y;
                bool condY2 = distanceY2 >= 0 && distanceY2 <= width;

                bool cond = (condX || condX2 || condY || condY2) && condH && condV;
                
                return cond;
            }
            ENDHLSL
        }
    }
}
