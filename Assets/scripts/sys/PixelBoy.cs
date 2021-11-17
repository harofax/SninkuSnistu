//PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
// - edited by @Nothke to use screen height for #LOWREZJAM

// ^ Original credit in the file, don't blame me please ^

using System;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
    public int h = 64;
    private int w;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        float ratio = ((float)cam.pixelWidth) / (float)cam.pixelHeight;
        w = Mathf.RoundToInt(h * ratio);
    }

    private void Update()
    {
 
        
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