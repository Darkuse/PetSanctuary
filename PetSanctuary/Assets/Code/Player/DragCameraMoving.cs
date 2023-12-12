using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCameraMoving : MonoBehaviour
{
    public float dragSpeed = 0.05f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    public Camera targetCamera; 
    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // When the left mouse button is pressed
        {
            dragOrigin = targetCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) // While the left mouse button is held down
        {
            Vector3 difference = dragOrigin - targetCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = targetCamera.transform.position + difference;
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

            targetCamera.transform.position = newPosition;
        }
    }

}
