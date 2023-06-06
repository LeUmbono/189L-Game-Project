using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TransitionEffect : MonoBehaviour
{
    public Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
    }
}
