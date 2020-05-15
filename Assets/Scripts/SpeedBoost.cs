using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public bool speedboostAquired = false;
    Collider2D _collider;
    SpriteRenderer _sp;
    AudioSource _as;
    PlayerMovement2D _pMove;
    bool playerEntered = false;

    // Start is called before the first frame update
    void Start()
    {
        
        _collider = GetComponent<Collider2D>();
        _sp = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerEntered == true)
        {
            PickUpInteractboost();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerEntered = true;
            _pMove = col.gameObject.GetComponent<PlayerMovement2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerEntered = false;
            _pMove = null;
        }
    }

    public void PickUpInteractboost()
    {
        Debug.Log("Player Here");
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Speed Boost Aquired");
            speedboostAquired = true;
            _sp.enabled = false;
            _collider.enabled = false;
            _as.Play();


        }
    }

   
}
