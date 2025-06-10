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
		public MeshDrawSettings DrawMeshParams { get; private set; }

		[field: SerializeField]
		public Vector3 Scale { get; private set; }
	}

	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Ability")]
	public class AbilityRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings Range { get; private set; }

		[field: SerializeField]
		public AbilityDirectionRendererData Direction { get; private set; }
	}
}
