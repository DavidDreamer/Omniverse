Shader "Omniverse/HealthBar"
{
    Properties
    {
        BackgroundColor ("BackgroundColor", Color) = (0,0,0,1)
        BaseColor ("BaseColor", Color) = (1,1,1,1)
        SecondColor ("SecondColor", Color) = (1,1,1,1)
        Scale ("Scale", Vector) = (1,1,1,1)
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
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            
            uniform float4 BackgroundColor;
            uniform float4 BaseColor;
            uniform float4 SecondColor;
            uniform float4 Scale;
            
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, Amount)
            UNITY_INSTANCING_BUFFER_END(Props)
            
            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 tex : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 tex : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            VertexOutput Vert(VertexInput input)
            {
                VertexOutput output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                
                output.pos = mul(UNITY_MATRIX_P,
                                 mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                                 + float4(input.vertex.x, input.vertex.y, 0.0, 0.0)
                                 * float4(Scale.x, Scale.y, 1.0, 1.0));

                output.tex = input.tex;

                return output;
            }

            float4 Frag(VertexOutput input) : COLOR
            {
                UNITY_SETUP_INSTANCE_ID(input);
                const float amount = UNITY_ACCESS_INSTANCED_PROP(Props, Amount);

                const float hClip = input.tex.x >= Scale.z && input.tex.x <= 1 - Scale.z;
                const float VClip = input.tex.y >= Scale.w && input.tex.y <= 1 - Scale.w;
                const float totalClip = hClip && VClip;
                const float factor = totalClip && step(input.tex.x, amount);

                const float4 combinedColor = lerp(BaseColor, SecondColor, input.tex.y);
                
                return lerp(BackgroundColor, combinedColor, factor);
            }
            ENDCG
        }
    }
}