using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public string triggerTag;
    public bool playerEntered;



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == triggerTag)
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == triggerTag)
        {
            playerEntered = false;
        }
    }
}
