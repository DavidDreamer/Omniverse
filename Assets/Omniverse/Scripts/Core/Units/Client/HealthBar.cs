using System.Linq;
using UnityEngine;

namespace Omniverse
{
	public class HealthBar: MonoBehaviour
	{
		private static class ShaderVariables
		{
			public static int Amount { get; } = Shader.PropertyToID(nameof(Amount));
		}

		[field: SerializeField]
		private MeshRenderer MeshRenderer { get; set; }
		
		private Property Property { get; set; }
		
		public void Initialize(Unit unit)
		{
			Property = unit.Properties.Values.First();
		}

		public void LateUpdate()
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetFloat(ShaderVariables.Amount, Property.Amount.Value / Property.Capacity.Value);
			MeshRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
