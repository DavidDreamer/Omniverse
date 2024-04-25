using System.Linq;
using UnityEngine;
using VContainer;

namespace Omniverse.Client
{
	public class HealthBar: MonoBehaviour
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));

			public static int SecondColor { get; } = Shader.PropertyToID(nameof(SecondColor));

			public static int Amount { get; } = Shader.PropertyToID(nameof(Amount));
		}

		[field: SerializeField]
		private HealthBarConfig Config { get; set; }

		[field: SerializeField]
		private MeshRenderer MeshRenderer { get; set; }

		private Property Property { get; set; }

		[Inject]
		private Player Player { get; set; }

		private Unit Unit { get; set; }

		public void Initialize(Unit unit)
		{
			Unit = unit;

			Property = Unit.Properties.Values.First();

			UpdateColors();
		}

		private void UpdateColors()
		{
			HealthBarColors colors = Player.FactionID == Unit.FactionID ? Config.AllyColors : Config.EnemyColors;
			var materialPropertyBlock = new MaterialPropertyBlock();
			MeshRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetColor(ShaderVariables.BaseColor, colors.BaseColor);
			materialPropertyBlock.SetColor(ShaderVariables.SecondColor, colors.SecondColor);
			MeshRenderer.SetPropertyBlock(materialPropertyBlock);
		}

		public void LateUpdate()
		{
			var materialPropertyBlock = new MaterialPropertyBlock();
			MeshRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat(ShaderVariables.Amount, Property.Amount.Value / Property.Capacity.Value);
			MeshRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
