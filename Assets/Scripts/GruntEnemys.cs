using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemys : MonoBehaviour
{
    public Transform waypoint1; //target points of enemy
    public Transform waypoint2; // target points of enemy
    public float moveSpeed = 2f;
    public float acceleration = 20f;

    Rigidbody2D _rb;
    Collider2D _c;
    AudioSource _as;
    Transform target;
    SpriteRenderer _sr;
    Animator _anim;
    int direction = 1;
    bool isDead = false;
    bool playerDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _c = GetComponent<Collider2D>();
        _as = GetComponent<AudioSource>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        target = waypoint1;
        _sr.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
    }
    
    void FixedUpdate()
    {
        if (isDead == false && GameState.playerIsDead == false)
        {
            Patrol();
        }
        else if (GameState.playerIsDead == true && playerDeath == false)
        {
            _rb.velocity = Vector2.zero;
            playerDeath = true;
        }
        /*else
        {
            _rb.velocity = Vector2.zero;
        }*/
        
    }
    
    void Patrol() // enemy patrol actions
    {
        _rb.AddForce(transform.right * direction * acceleration, ForceMode2D.Force);
        _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -moveSpeed, moveSpeed), _rb.velocity.y);
    }
    void UpdateTarget()
    {
        if(Mathf.Abs(transform.position.x - target.position.x) < 0.1f)
        {
            direction = -direction;
            _sr.flipX = !_sr.flipX;
            if(target == waypoint1)
            {
                target = waypoint2;
            }
            else
            {
                target = waypoint1;
            }
        }
    }

    public void Die() // Enemy death actions
    {
        Debug.Log("Dead");
        isDead = true;
        _c.enabled = false;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _sr.flipY = true;
        _sr.flipX = !_sr.flipX;
        _anim.enabled = false;
        _rb.AddForce(transform.up * 5, ForceMode2D.Impulse);
        _as.Play();
        Destroy(gameObject, 5);
    }
}
