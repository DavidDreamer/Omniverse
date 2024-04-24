using Omniverse.Mapping;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
    public class MiniMapWidget : MonoBehaviour
    {
        public RawImage Image;

        public MiniMapCameraBounds CameraBounds;
        
        [Inject]
        private Map Map { get; set; }
   
        void Start()
        {
            Image.texture = Map.RenderTexture;
        }
    }
}