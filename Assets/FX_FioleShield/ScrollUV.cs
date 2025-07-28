using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{

    public bool Scroll = false;
    public float ScrollX = 0.5f;
    public float scrollY = 0.5f;


    // Update is called once per frame
    void Update()
    {

        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * scrollY;
        if (Scroll) GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);

    }
}
