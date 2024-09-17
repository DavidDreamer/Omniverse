Shader "Omniverse/Utility/SelectionBox"
{
    Properties
    {
        FrameColor ("FrameColor", Color) = (1,1,1,1)
        FrameWidth ("FrameWidth", Float) = 5
        FillingColorTop ("FillingColorTop", Color) = (1,1,1,1)
        FillingColorBottom ("FillingColorBottom", Color) = (1,1,1,1)
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
            float FrameWidth;

            float4 FillingColorTop;
            float4 FillingColorBottom;

            float4 SelectionBox;
           
            bool IsPointInsideRect(const float2 position, const float4 rect)
            {
                return position.x >= rect.x && position.x <= rect.z && position.y >= rect.y && position.y <= rect.w;
            }

            float4 frag (v2f i) : SV_Target
            {
                const float2 position = i.vertex;

                if (IsPointInsideRect(position, SelectionBox))
                {
                    float4 selectionBoxWithoutFrame = SelectionBox + float4(FrameWidth, FrameWidth, -FrameWidth, -FrameWidth);

                    if (IsPointInsideRect(position, selectionBoxWithoutFrame))
                    {
                        const float height = (position.y - selectionBoxWithoutFrame.y) / (selectionBoxWithoutFrame.w - selectionBoxWithoutFrame.y);
                        return lerp(FillingColorBottom, FillingColorTop, height);
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