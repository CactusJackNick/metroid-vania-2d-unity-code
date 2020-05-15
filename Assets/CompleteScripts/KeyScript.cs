using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool haveKey = false; 

    Collider2D _collider;
    SpriteRenderer _sp;
    AudioSource _as;
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
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerEntered = false;
        }
    }

    void PickUpInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            haveKey = true;
            Debug.Log("Key Aquired");
            _sp.enabled = false;
            _collider.enabled = false;
            _as.Play();
        }
    }
}
