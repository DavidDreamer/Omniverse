#pragma once
#include <HLSLSupport.cginc>

CBUFFER_START(FogOfWarProperties)
float4 FogOfWarUnexploredColor;
float4 FogOfWarExploredColor;
float FogOfWarAnimationSpeed;
CBUFFER_END

uniform sampler2D FogOfWarTexture;

uniform StructuredBuffer<int> CellsVisibilityBuffer;

#define CELL_VISIBILITY_UNEXPLORED 0
#define CELL_VISIBILITY_EXPLORED 1
#define CELL_VISIBILITY_VISIBLE 2