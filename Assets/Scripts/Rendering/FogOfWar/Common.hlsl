#pragma once
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

uniform float4 FogOfWarResolution;

uniform sampler2D FogOfWarTexture;

CBUFFER_START(FogOfWarProperties)
float4 FogOfWarUnexploredColor;
float4 FogOfWarExploredColor;
float FogOfWarAnimationSpeed;
float FogOfWarBorderLength;
CBUFFER_END

uniform StructuredBuffer<int> CellsVisibilityBuffer;

#define CELL_VISIBILITY_UNEXPLORED 0
#define CELL_VISIBILITY_EXPLORED 1
#define CELL_VISIBILITY_VISIBLE 2