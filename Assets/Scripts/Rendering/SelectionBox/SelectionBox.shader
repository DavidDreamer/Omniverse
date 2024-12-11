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
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float4 FrameColor;
            float FrameWidth;

            float4 FillingColorTop;
            float4 FillingColorBottom;

            float4 SelectionBox;
           
            bool IsPointInsideRect(const float2 position, const float4 rect)
            {
                return position.x >= rect.x && position.x <= rect.z && position.y >= rect.y && position.y <= rect.w;
            }

             float4 Frag(Varyings input) : SV_Target
            {
                const float2 position = input.positionCS.xy;

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
            ENDHLSL
        }
    }
}