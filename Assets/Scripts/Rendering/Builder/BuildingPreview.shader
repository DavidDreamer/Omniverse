Shader "Omniverse/Builder/Preview"
{
    Properties
    {
        BaseColor ("BaseColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        Cull Off

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
                float4 vertex : SV_POSITION;
                float opacity : TEXCOORD0;
            };

            uniform float3 _BuilderBoundsSize;

            float4 BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.opacity = (v.vertex.y / _BuilderBoundsSize.y);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = float4(BaseColor.rgb, BaseColor.a * i.opacity);
                return col;
            }
            ENDCG
        }
    }
}
