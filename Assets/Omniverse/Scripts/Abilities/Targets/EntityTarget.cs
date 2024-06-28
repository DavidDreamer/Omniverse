using System;
using Dreambox.Core;
using Omniverse.Entities;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class ResourceSourceTargetDesc
	{
		[field: SerializeField]
		public ResourceDesc[] Resources { get; private set; }
	}
	
	[Serializable]
	public class UnitTargetDesc
	{
		[field: SerializeField]
		public EntityTargetType Filter { get; private set; }
	}
	
	public class EntityTarget: ITarget
	{
		[field: SerializeField]
		public float Range { get; private set; }
		
		[field: SerializeReference]
		[field: Optional]
		public ResourceSourceTargetDesc ResourceSources { get; private set; }
		
		[field: SerializeReference]
		[field: Optional]
		public UnitTargetDesc Units { get; private set; }
	}
}
