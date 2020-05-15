using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatRight : MonoBehaviour
{
    public IEnumerator MovePlatformRight() // Moving Platform to the right
    {
        Vector3 target = new Vector3(transform.position.x + 3f, transform.position.y, 0);
        while (Vector3.Distance(target, transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, 10 * Time.deltaTime);
            Debug.Log("Lerping");
            yield return null;
        }
    }
}
