using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDepthSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(-transform.position.y * 100);
    }
}
