using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector2 newPosition = player.position; 
        newPosition.x = transform.position.x;
        //transform.position = newPosition;
    }
}
