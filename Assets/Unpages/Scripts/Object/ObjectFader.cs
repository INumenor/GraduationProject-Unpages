using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material mat;
    public bool doFade = false;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;
    }
    void Update()
    {
        if (doFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }

    }
    void FadeNow()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r,currentColor.g,currentColor.b,Mathf.Lerp(currentColor.a,fadeAmount,fadeSpeed));
        mat.color = smoothColor;
    }
    void ResetFade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));
        mat.color = smoothColor;
    }
}
