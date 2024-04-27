using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Camera
{
	public class FogOfWarRenderer: MonoBehaviour, IInitializable, IDisposable
	{
		[Inject]
		private FogOfWar FogOfWar { get; set; }
		
		public Texture2D texture;

		public RenderTexture RenderTexture;
		public RenderTexture RenderTexture2;

		public Material Material;

		private Material BlurMaterial;

		public float Radius;
		
		public float alpha;
		
		public bool ApplyBlur;

		public void Initialize()
		{
			texture = new Texture2D(FogOfWar.Resolution.x, FogOfWar.Resolution.y);

			RenderTexture = CreateRenderTexture("FogOfWar0");
			RenderTexture2 = CreateRenderTexture("FogOfWar1");
			
			Material.SetTexture("_BaseMap", RenderTexture2);

			BlurMaterial = new Material(Shader.Find("Hidden/Dreambox/Blur"));
			BlurMaterial.SetFloat("Factor", 1);
			BlurMaterial.SetKeyword(new LocalKeyword(BlurMaterial.shader, "ALGORITHM_GAUSSIAN"), true);

			RenderTexture CreateRenderTexture(string textureName)
			{
				return new RenderTexture(FogOfWar.Resolution.x, FogOfWar.Resolution.y,
					GraphicsFormat.R16G16B16A16_SFloat,
					GraphicsFormat.None)
				{
					name = textureName,
					anisoLevel = 0
				};
			}
		}

		public void Dispose()
		{
			RenderTexture.Release();
			RenderTexture2.Release();
		}

		public void LateUpdate()
		{
			BlurMaterial.SetFloat("Radius", Radius);
			
			for (int x = 0; x < FogOfWar.Resolution.x; ++x)
			{
				for (int y = 0; y < FogOfWar.Resolution.y; ++y)
				{
					FogOfWarCell cell = FogOfWar.Cells[x, y];
					
					Color color = new Color(0, 0, 0, cell.Value * alpha);
					
					texture.SetPixel(x, y, color);
				}
			}
			
			texture.Apply();

			if (ApplyBlur)
			{
				Graphics.Blit(texture, RenderTexture, BlurMaterial, 0);
				Graphics.Blit(RenderTexture, RenderTexture2, BlurMaterial, 1);
			}
			else
			{
				Graphics.Blit(texture, RenderTexture2);
			}
		}
	}
}
