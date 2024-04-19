using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NavigationPoint: MonoBehaviour
{
	private static int PowerID { get; } = Shader.PropertyToID("Power");
	
	[field: SerializeField]
	private DecalProjector DecalProjector { get; set; }

	private void Awake()
	{
		DecalProjector.material = new Material(DecalProjector.material);
	}

	public void SetPower(float power) => DecalProjector.material.SetFloat(PowerID, power);
}
