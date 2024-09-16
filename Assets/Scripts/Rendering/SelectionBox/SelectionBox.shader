Shader "Omniverse/Utility/SelectionBox"
{
    Properties
    {
        FrameColor ("FrameColor", Color) = (1,1,1,1)
        FrameWidth ("FrameWidth", Float) = 5
        FillingColor ("FillingColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 FrameColor;
            float4 FillingColor;

            float4 SelectionBox;
           
            bool IsPointInsideRect(const float2 position, const float4 rect)
            {
                return position.x >= rect.x && position.x <= rect.z && position.y >= rect.y && position.y <= rect.w;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 selectionBoxWithoutFrame = SelectionBox + float4(1, 1, -1, -1);
                const float2 position = i.vertex;
                if (IsPointInsideRect(position, SelectionBox))
                {
                    if (IsPointInsideRect(position, selectionBoxWithoutFrame))
                    {
                        const float alpha = (position.y - selectionBoxWithoutFrame.y) / (selectionBoxWithoutFrame.w - selectionBoxWithoutFrame.y);
                        return FillingColor * alpha;
                    }
                    else
                    {
                        return FrameColor;
                    }
                }
                else
                {
                    return 0;
                }
            }
            ENDCG
        }
    }
}