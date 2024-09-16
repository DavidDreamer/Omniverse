Shader "Omniverse/Utility/SelectionBox"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform float4 SelectionBox;
           
            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = float4(0, 1, 0, 0.5);

                if (i.vertex.x >= SelectionBox.x && i.vertex.x <= SelectionBox.z && i.vertex.y >= SelectionBox.y && i.vertex.y <= SelectionBox.w)
                {
                    return color;
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }
            ENDCG
        }
    }
}
