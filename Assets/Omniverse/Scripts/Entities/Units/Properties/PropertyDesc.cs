using System;
using Dreambox.Core;
using UnityEngine;

namespace Omniverse.Entities.Units
{
	[Serializable]
	public class PropertyDesc
	{
		[field: SerializeField]
		public PropertyID ID { get; private set; }

		[field: SerializeField]
		public float Default { get; private set; }
		
		[field: SerializeField]
		public FloatRange Range { get; private set; }
		
		[field: SerializeField]
		public float Regeneration { get; private set; }

		[field: SerializeField]
		public bool Vital { get; private set; }
	}
}
