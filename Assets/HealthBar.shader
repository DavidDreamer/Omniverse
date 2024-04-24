Shader "Omniverse/HealthBar"
{
    Properties
    {
        BackgroundColor ("BackgroundColor", Color) = (0,0,0,1)
        MainColor ("MainColor", Color) = (1,1,1,1)
        Scale ("Scale", Vector) = (1,1,1,1)
        Amount ("Amount", float) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            uniform float4 BackgroundColor;
            uniform float4 MainColor;
            uniform float4 Scale;
            uniform float Amount;

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 tex : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 tex : TEXCOORD0;
            };
            
            VertexOutput Vert(VertexInput input)
            {
                VertexOutput output;
                
                output.pos = mul(UNITY_MATRIX_P,
                                 mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                                 + float4(input.vertex.x, input.vertex.y, 0.0, 0.0)
                                 * float4(Scale.x, Scale.y, 1.0, 1.0));

                output.tex = input.tex;

                return output;
            }

            float4 Frag(VertexOutput input) : COLOR
            {
                const float hClip = input.tex.x >= Scale.z && input.tex.x <= 1 - Scale.z;
                const float VClip = input.tex.y >= Scale.w && input.tex.y <= 1 - Scale.w;
                const float totalClip = hClip && VClip;
                const float factor = totalClip && step(input.tex.x, Amount);
                return lerp(BackgroundColor, MainColor, factor);
            }
            ENDCG
        }
    }
}