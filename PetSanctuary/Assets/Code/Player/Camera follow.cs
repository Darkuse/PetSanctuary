using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public Vector3 offset; // Offset of the camera from the player

    private void LateUpdate()
    {
        // Update the camera position to follow the player
        transform.position = target.position + offset;
    }
}
