Shader "Omniverse/Builder/Grid"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _GridColor ("Grid Color", Color) = (1,1,1,1)
        _RenderingLayer ("Rendering Layer", Integer) = 2
        _Radius ("Radius", Integer) = 3
        _Thickness ("Thickness", Range(0, 1)) = 0.05
    }
    SubShader
    {
        Cull Off 
        ZWrite Off 
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareRenderingLayerTexture.hlsl"

            float4 _MainColor;
            float4 _GridColor;
            uint _RenderingLayer;
            uint _Radius;
            float _Thickness;

            uniform float3 _BuilderBoundsSize;
            uniform float3 _BuilderFocusPoint;
            
            float SouldBeCulledByLayer(float4 positionCS)
            {
                #ifdef _RENDER_PASS_ENABLED
                    uint surfaceRenderingLayer = DecodeMeshRenderingLayer(LOAD_FRAMEBUFFER_X_INPUT(GBUFFER4, positionCS.xy).r);
                #else
                    uint surfaceRenderingLayer = LoadSceneRenderingLayer(positionCS.xy);
                #endif

                return (surfaceRenderingLayer & _RenderingLayer) - 0.1;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                clip(SouldBeCulledByLayer(input.positionCS));

                float2 UV = input.positionCS.xy / _ScaledScreenParams.xy;

                // Sample the depth from the Camera depth texture.
                #if UNITY_REVERSED_Z
                real depth = SampleSceneDepth(UV);
                #else
                // Adjust Z to match NDC for OpenGL ([-1, 1])
                real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
                #endif
                
                float3 worldPos = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);

                float3 radius = _BuilderBoundsSize + _Radius;
                float opacityFromDistance = 1 - saturate(length(_BuilderFocusPoint - worldPos) / radius.x);

                float2 worldPosFrac = frac(worldPos.xz);
                float2 aa = saturate(worldPosFrac / _Thickness);
                float2 bb = saturate((1 - worldPosFrac) / _Thickness);

                float h = 1 - min(aa.x, bb.x);
                float v = 1 - min(aa.y, bb.y);
                float gridFactor = lerp(h, 1, v);

                float4 finalColor = lerp(_MainColor, _GridColor, gridFactor);

                return finalColor * opacityFromDistance;
            }
            ENDHLSL
        }
    }
}
