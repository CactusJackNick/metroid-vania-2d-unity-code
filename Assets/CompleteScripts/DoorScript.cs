using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    
    public KeyScript doorKey;
    public AudioClip doorLocked;
    public AudioClip doorOpen;


   // Collider2D _collider;
    //SpriteRenderer _sp;
    AudioSource _as;
    TriggerScript _ts;
    bool doorComplete;
    //bool playerEntered;
    

    // Start is called before the first frame update
    void Start()
    {
       // _collider = GetComponent<Collider2D>();
        //_sp = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        _ts = GetComponentInChildren<TriggerScript>();
    }

    void Update()
    {
        if (_ts.playerEntered == true)
        {
            DoorInteract();
        }
    }

    
    /*private void OnTriggerEnter2D(Collider2D col)
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
    }*/

    private void DoorInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && doorComplete == false)
        {
            if (doorKey != null)
            {


                if (doorKey.haveKey == true)
                {
                    OpenDoor();
                }
                else
                {
                    _as.clip = doorLocked;
                    _as.Play();
                }
            }
            else
            {
                OpenDoor();
            }
        }
    }
        void OpenDoor()
        {
            Debug.Log("OpenDoor");
            //_sp.enabled = false;
            //_collider.enabled = false;
            _as.clip = doorOpen;
            _as.Play();
            doorComplete = true;
        StartCoroutine(MoveDoor());
        }

    IEnumerator MoveDoor()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y + 3, 0);
        while(Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, 1 * Time.deltaTime);
            Debug.Log("Lerping");
            yield return null;
        }
        transform.position = target;
        yield return new WaitForSeconds(3f);
        Debug.Log("Door Done");
    }
}
