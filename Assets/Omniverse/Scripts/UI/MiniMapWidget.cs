using Omniverse.Mapping;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
    public class MiniMapWidget : MonoBehaviour
    {
        public RawImage Image;

        [Inject]
        private MiniMap MiniMap { get; set; }
   
        void Start()
        {
            Image.texture = MiniMap.RenderTexture;
        }
    }
}