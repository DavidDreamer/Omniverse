using Omniverse.Mapping;
using Omniverse.FogOfWar.Rendering;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
    public class MiniMapWidget : MonoBehaviour
    {
        public RawImage Image;
        public RawImage FogOfWar;

        public MiniMapCameraBounds CameraBounds;
        
        [Inject]
        private Map Map { get; set; }
   
        [Inject]
        private FogOfWarRenderer FogOfWarRenderer { get; set; }
        
        void Start()
        {
            Image.texture = Map.RenderTexture;
            FogOfWar.texture = FogOfWarRenderer.RenderTexture2;
        }
    }
}