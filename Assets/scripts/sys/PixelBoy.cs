//PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
// - edited by @Nothke to use screen height for #LOWREZJAM

// ^ Original credit in the file, don't blame me please ^
using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
    public int h = 64;
    private int w;

    private void Update()
    {
 
        float ratio = ((float)Camera.main.pixelWidth) / (float)Camera.main.pixelHeight;
        w = Mathf.RoundToInt(h * ratio);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}