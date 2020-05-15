using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunPickUp : MonoBehaviour
{
    Collider2D _collider;
    SpriteRenderer _sp;
    AudioSource _as;
    Shotgun _sg;
    bool playerEntered;

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
            PickUpInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerEntered = true;
            _sg = col.gameObject.GetComponentInChildren<Shotgun>();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerEntered = false;
            _sg = null;
        }
    }

    void PickUpInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _sg.shotgunPicked = true;
            _sg.shotgunActivate();
            _sp.enabled = false;
            _collider.enabled = false;
            _as.Play();
        }
    }
}
