using System;
using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[Serializable]
	public class AbilityDirectionRendererData
	{
		[field: SerializeField]
		public MeshDrawSettings MeshDrawSettings { get; private set; }

		[field: SerializeField]
		public Vector3 Scale { get; private set; }
	}

	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Ability")]
	public class AbilityRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings Range { get; private set; }

		[field: SerializeField]
		public AbilityDirectionRendererData Direction { get; private set; }
	}
}
