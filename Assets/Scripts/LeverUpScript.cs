using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverUpScript : MonoBehaviour
{
    public MovingPlatUp[] PlatScriptUp; // Up Movement Platform script
    public Sprite crankup; // first state of lever
    public Sprite crankdown; // second state of lever
    public bool leverPulled = false; //checks if the lever is pulled
    public bool statepulled = false; //checks lever's state
    public AudioClip leverpulledaudio; //leveraudio
    AudioSource _as;
    SpriteRenderer _sr;
    TriggerScript _ts;
    Collider2D collider2d;
   


    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _sr = GetComponent<SpriteRenderer>();
        _ts = GetComponentInChildren<TriggerScript>();
        collider2d = GetComponent<Collider2D>();
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
            leverPulled = true;

            if (Input.GetKeyDown(KeyCode.E) && _ts.playerEntered == true && statepulled == false) //input of the player
            {
                PullLeverUp();
                statepulled = true;
                collider2d.enabled = false;
            }
        }
    }

    void PullLeverUp() //Actions that happen when pulling the lever
    {
        GetComponent<SpriteRenderer>().sprite = crankdown;
        _as.clip = leverpulledaudio;
        _as.Play();

        PlatScriptUp[0].StartCoroutine("MovePlatformUp");
        

    }

    
}
