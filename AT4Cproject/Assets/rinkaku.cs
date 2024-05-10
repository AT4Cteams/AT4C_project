using UnityEngine;
using System.Collections;

public class rinkaku : MonoBehaviour
{

    public Material monoTone;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, monoTone);
    }
}