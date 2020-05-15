using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject _player; //Stores player avatar
    public float smooth = 1f; //How Smooth the camera follows the player

    float lastPlayerX = 0; // Players position in x on the last frame
    float camLeadX = 0; // Amount the camera will be infront of the player
    float camTargetX = 0; // The cameras target x value.

    // FixedUpdate is called once per time interval
    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        if (_player != null && GameState.playerIsDead == false) // Checks to see the player exists
        {
            if (lastPlayerX < _player.transform.position.x && _player.transform.position.x - lastPlayerX > 0.1f) // Checks to see if the player has moved significantly right to change the camera's target
            {
                camLeadX = 6;
            }
            else if(lastPlayerX > _player.transform.position.x && lastPlayerX - _player.transform.position.x > 0.1f) // Checks to see if the player has moved significantly left to change the camera's target
            {
                camLeadX = -6;
            }

            camTargetX = Mathf.Lerp(transform.position.x, _player.transform.position.x + camLeadX, smooth); //Smoothly changes cameras x target
            Vector3 target = new Vector3(camTargetX, _player.transform.position.y + 2, -10); //Sets overall target for camera
            transform.position = Vector3.Lerp(transform.position, target, smooth); //Smoothly moves camera to target
            lastPlayerX = _player.transform.position.x; //Sets the plays position this frame ready for the next frame
        }
    }
}
