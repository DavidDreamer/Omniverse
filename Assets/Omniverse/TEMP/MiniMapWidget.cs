using Omniverse;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
