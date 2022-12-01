using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetAnimation : MonoBehaviour
{
    public float offsetx = 0.1f;
    public float offsety = 0.1f;
    float offsetchangex = 0f;
    float offsetchangey = 0f;
    float time = 0.25f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(AnimateOffset());
    }

    IEnumerator AnimateOffset()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            offsetchangex += offsetx;
            offsetchangey += offsety;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offsetchangex, offsetchangey / 2));
        }
    }
}