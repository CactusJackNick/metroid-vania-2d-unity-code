using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTwo : MonoBehaviour
{
    public MovingPlat[] PlatScript; //left platform script movement
    public MovingPlatRight[] PlatScriptRight; // right platfrom script movement
    public Sprite crankup; // first state of lever
    public Sprite crankdown; // second state of lever
    public bool leverPulled = false;

    public AudioClip leverpulledaudio; //leveraudio
    AudioSource _as;
    TriggerScript _ts;
    Collider2D col2D;



    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _ts = GetComponentInChildren<TriggerScript>();
        col2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        leverInteract();
    }


    private void leverInteract() // checking for player-lever interaction
    {
        if (_ts.playerEntered == true)
        {


            if (Input.GetKeyDown(KeyCode.E) && leverPulled == false) // input of the player
            {
                PullLever();
                leverPulled = true;
                col2D.enabled = false;
            }
        }
    }

    void PullLever() //Actions that happen when pulling the lever
    {
        GetComponent<SpriteRenderer>().sprite = crankdown;
        _as.clip = leverpulledaudio;
        _as.Play();
        PlatScript[0].StartCoroutine("MovePlatform");
        PlatScript[1].StartCoroutine("MovePlatform");

    }
}
