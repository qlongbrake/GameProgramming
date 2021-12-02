using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraControl : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = player.position.y + 10;
        transform.position = newPosition;
    }
}
