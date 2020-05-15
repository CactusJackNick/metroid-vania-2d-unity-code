using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a base player movement script for GAD4001 for students to use as the bassis of their games
 There is an expectation that students will add functionality to this character controller and seek 
 to build upon what is here. Happy Scripting - Ewan Armstrong - GAD4001 Tutor*/

public class PlayerMovement2D : MonoBehaviour
{
    public float jumpHeight = 10f; // Effects the height of the player
    public float moveSpeed = 10f; // Effects the maximum speed of the player
    public float acceleration = 20f; // The players acceleration to moveSpeed
    public bool airMove = false; //Allows the player to move side to side whilst in the air
    public float airMovePenalty = 0.5f; //The penalty for moving in the 0 = no movement 1 = full movement
    public float coyoteTime = 0.2f;
    public int numberOfJumps = 1; //Number of times the player can jump befofre returning to the ground
    public int numberOfJumpsAfterFall = 1; //Number of times the player can jump after falling
    public AudioSource _asJump; //Jump sound source
    public AudioSource _asLand; //Land sound source
    private float normalSpeed; //actual speed
    public float speedCooldown; //duration of the boost
    public float newTempSpeed = 20f; //boosted speed

    Rigidbody2D _rb; //Player rigidbody 
    Animation _anim; //Player animation
    Animator _animator; //Player animator
    SpriteRenderer _sr;
    bool jumpPlaying = false;
    bool jumping = false; //State of is player jumping
    bool inAir = true; //State of is the player in the air
    float movement = 0f; //Stores the player input
    bool noRightMove = false; //Prevents the player from moving right
    bool noLeftMove = false; //Prevents the player from moving left
    bool inCoyoteTime = false;
    float coyoteTimeEnd = 0;
    int jumpCount = 0; //Number of times the player has jumped
    LayerMask mask; //Mask to stop boxCast hitting the player
    public SpeedBoost speedBooster; // reference to speedboost script

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //Assigns local Rigidbody
        _anim = GetComponent<Animation>(); //Assigns local Animation
        mask = LayerMask.GetMask("Player"); //Sets LayerMask to ignore Player and Interactable layer
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        normalSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        BoostSpeed();
        BoxRay(); //Checks collisions first
        Coyote(); //Allows the player to jump a small amount of time after falling
        Inputs(); //Checks for player input
        JumpStateMachine(); //Applies jump motion
    }

    void Coyote() // Allows the player a short window to still jump after begining to fall
    {
        if (inCoyoteTime == true)
        {
            Debug.Log("In coyote time");
            if (Time.time > coyoteTimeEnd)
            {
                Debug.Log("Coyote time end");
                inAir = true;
                inCoyoteTime = false;
            }
        }
    }

    //Creates box cast around the player
    void BoxRay()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y - 0.04f); //Stores player's 3D location into 2D (only x and y needed)
        //RaycastHit2D[] hits = Physics2D.BoxCastAll(pos, new Vector2(0.5f * (transform.localScale.x / 2.9f), 0.5f * (transform.localScale.y / 2.9f)), 0, Vector2.zero, Mathf.Infinity, ~mask); //casts a box ray into the scene and stores all objects hit in an array.
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(pos, new Vector2(1.1f, 1.6f),CapsuleDirection2D.Vertical, 0, Vector2.zero, Mathf.Infinity, ~mask); //casts a box ray into the scene and stores all objects hit in an array.
        noRightMove = false; //Allows player to move right
        noLeftMove = false; //Allows player move left
                
        if (hits.Length == 0)//if nothing is hit, set inAir to true and do nothing else.
        {
            if (inAir == false && inCoyoteTime == false)
            {
                coyoteTimeEnd = Time.time + coyoteTime;
                inCoyoteTime = true;
            }
            //inAir = true; This is what used to happen
        }
        else //Loop through all the hits every frame
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Vector2 conPoint = hits[i].point; //Get the point at which the player hit the ground and store it in a variable
                Vector2 dif = conPoint - pos; //Get the vector betwen the player location and the hit point
                float angY = Vector2.Angle(transform.up, dif); //Gets the angle between up on the character and the hit point
                float angX = Vector2.Angle(transform.right, dif); //Gets the angle between right on the character and the hit point

                if (hits[i].collider.tag == "Ground") //If the player hit the ground check angle of hit, else do nothing
                {
                    if (angY >= 135) // If the angY is greater than 135, we know the bottom has been hit
                    {
                        if (inAir && _rb.velocity.y <= 0.1f)  // If inAir is true and velocity is very low, we have just landed
                        {
                            inAir = false;// Make inAir fales; we are on the ground
                            //Play animations and sounds
                            if (!_asLand.isPlaying)
                            {
                                
                                _anim.Stop();
                                _anim.Play("PlayerLand");
                                PlaySound(_asLand);
                            }
                        }

                        if(jumpPlaying == true && _rb.velocity.y <= 0.1f)
                        {
                            JumpAnimPlay(false);
                        }
                        inCoyoteTime = false;
                        if (!jumping && _rb.velocity.y <= 0) //If player is not pressing to jump and the velocity is less than or equal to zero in the vertical, reset jump count
                        {
                            jumpCount = 0;
                        }
                    }
                    else if (angX < 45) //If player hits wall on right
                    {
                        if (_rb.velocity.x > 0 && !_asLand.isPlaying) //Plays sound if is not playing and the player is not moving right
                        {
                            PlaySound(_asLand);
                        }
                        noRightMove = true; //Prevents further movement right
                    }
                    else if (angX > 135) //If player hits wall onleft
                    {
                        if (_rb.velocity.x < 0 && !_asLand.isPlaying) //Plays sound if is not playing and the player is not moving left
                        {
                            PlaySound(_asLand);
                        }
                        noLeftMove = true; //Prevents further movement left
                    }
                }
                else if (hits[i].collider.tag == "Enemy")
                {
                    if (angY >= 135) // If the angY is greater than 135, we know the bottom has been hit and enemy dies
                    {
                        hits[i].collider.gameObject.BroadcastMessage("Die");
                        Debug.Log("Kill Enemy");
                        PerformJump();
                    }
                    else // Player dies
                    {
                        PlayerDeath();
                    }
                }
                else if (hits[i].collider.tag == "Hazard")
                {
                    PlayerDeath();
                }
            }
        }
    }

    void PlayerDeath()
    {
        _anim.enabled = false;
        gameObject.BroadcastMessage("Die");
        Debug.Log("Die");
    }
    //Gets player input from input manager and stores them in variables
    void Inputs()
    {
        movement = Input.GetAxis("Horizontal"); //Horizontal movement (A/D)
        jumping = Input.GetButtonDown("Jump"); //Jumping (Spacebar)

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _sr.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _sr.flipX = false;
        }
    }

    //Decides what to do when jump is pressed
    void JumpStateMachine()
    {
        if (jumping) //Jump pressed
        {
            if (jumpCount == 0 && inAir) //Works out the player has fallen of a ledge
            {
                jumpCount = numberOfJumps - numberOfJumpsAfterFall; //Ensures the number of jumps after fall is set Inspector
            }
            ++jumpCount; // Adds to jump counter
            if (jumpCount <= numberOfJumps) //Allows jump to be performed if jump count is less or equal to the allowed number
            {
                PerformJump(); //Runs function to perform jumps
            }
        }
    }

    //Allies force to make player jump
    void PerformJump()
    {
        JumpAnimPlay(true);
        inAir = true; //Stores state of player inAir
        _rb.velocity = new Vector2(_rb.velocity.x, 0); //Sets vertical velocity to zero. This allows jumps to be predictable by not adding to existing vertical forces
        _rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); //Applies force upwards to player
        _anim.Stop(); //Stops animation
        _anim.Play("PlayerJump"); //Plays jump animation
        PlaySound(_asJump); //Plays jump sound
    }

    void JumpAnimPlay(bool play)
    {
        if( play == true)
        {
            _animator.SetTrigger("StartJump");
            jumpPlaying = true;
        }
        else
        {
            _animator.SetTrigger("EndJump");
            jumpPlaying = false;
        }
    }



    //function to play a sound
    void PlaySound(AudioSource audio) //Allows for an audio source to be passed to it as a variable to play audio in different sources.
    {
        audio.Stop(); //Stops the source if already playing
        audio.pitch = Random.Range(0.59f, 0.61f); //Randomises the pitch a bit so it is less fatiguing
        audio.Play(); //Plays audio
    }

    //Runs player movment
    void Move()
    {
        if (noLeftMove) //Clamps input if player hits wall to their left
        {
            movement = Mathf.Clamp(movement, 0, 1);
        }
        else if (noRightMove)//Clamps input if player hits wall to their right
        {
            movement = Mathf.Clamp(movement, -1, 0);
        }

        if (!airMove && inAir) //Prevents air movement if chosen in Inspector 
        {
            movement = 0f;
        }
        else if (airMove && inAir) //Allows air movement if chosen in Inspector 
        {
            movement = movement * airMovePenalty; //Applies air penalty 
        }
        
        _rb.AddForce(transform.right * movement * acceleration, ForceMode2D.Force); //Applies force to move
        _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -moveSpeed, moveSpeed), _rb.velocity.y); //Caps speed to maximum
        _animator.SetFloat("Speed", Mathf.Abs(_rb.velocity.x)); 
    }

    void BoostSpeed()
    {
        if (speedBooster.speedboostAquired == true) //checks of the berries were picked and skill was aquired
        {
            if (Input.GetKeyDown(KeyCode.R)) //boosts speed temporarily
            {
                moveSpeed = newTempSpeed;
                StartCoroutine(SpeedDuration());
            }
        }
    }

    IEnumerator SpeedDuration() //duration of the boost
    {
        yield return new WaitForSeconds(speedCooldown);
        moveSpeed = normalSpeed;
    }

    //Called every interval of time
    void FixedUpdate()
    {
        Move(); //Move is called here as it applies force continuously
    }
}
