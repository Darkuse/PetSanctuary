using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDepthSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int difference = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(-transform.position.y * 10+difference);
    }
}
