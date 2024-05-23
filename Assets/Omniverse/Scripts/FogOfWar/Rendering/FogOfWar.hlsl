#pragma once
#include <HLSLSupport.cginc>

CBUFFER_START(FogOfWarProperties)
float4 FogOfWarUnexploredColor;
float4 FogOfWarConcealedColor;
CBUFFER_END

uniform sampler2D FogOfWarTexture;

#define CELL_VISIBILITY_UNEXPLORED 0
#define CELL_VISIBILITY_CONCEALED 1
#define CELL_VISIBILITY_VISIBLE 2
