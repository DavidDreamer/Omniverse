// Copyright (c) Saber BGS 2023. All rights reserved.
// ---------------------------------------------------------------------------------------------

using UnityEngine;

namespace Dreambox.Rendering
{
	[CreateAssetMenu(menuName = "Dreambox/Rendering/OutlineConfig", fileName = nameof(OutlineConfig))]
	public class OutlineConfig: ScriptableObject
	{
		[field: SerializeField] public Color OutlineColor { get; private set; } = Color.white;
		[field: SerializeField] public Color FillColor { get; private set; } = Color.clear;
		[field: SerializeField, Range(0f, 0.05f)] public float Width { get; private set; } = 0.0044f;
		[field: SerializeField, Range(0f, 1f)] public float Softness { get; private set; } = 0.4f;
		[field: SerializeField, Range(1f, 5f)] public float SoftnessPower { get; private set; } = 2f;
		[field: SerializeField, Range(0f, 1f)] public float PixelOffset { get; private set; } = 0f;
		[field: SerializeField] public Color FillFlickColor { get; private set; } = Color.clear;
		[field: SerializeField] public float FillFlickRate { get; private set; } = 6f;
		[field: SerializeField, Range(0f, 0.05f)] public float CutOffWidth { get; private set; } = 0;
	}
}
