using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public AudioClip deathHit;
    public AudioClip deathDrop;

    Rigidbody2D _rb;
    Collider2D _c;
    PlayerMovement2D _pm2D;
    AudioSource _as;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _c = GetComponent<Collider2D>();
        _pm2D = GetComponent<PlayerMovement2D>();
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        StartCoroutine(PlayerDrop());
    }

    IEnumerator PlayerDrop()
    {
        GameState.playerIsDead = true;
        _c.enabled = false;
        _pm2D.enabled = false;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;
        _as.clip = deathHit;
        _as.Play();

        yield return new WaitForSeconds(1f);

        _rb.gravityScale = 2;
        _rb.AddForce(transform.up * 7, ForceMode2D.Impulse);
        _as.clip = deathDrop;
        _as.Play();
    }
}
