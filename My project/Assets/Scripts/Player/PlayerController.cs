using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    //Component references
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //movement variables
    public float speed = 5.0f;
    public float jumpForce = 300.0f;

    //groundcheck stuff
    public bool isGrounded; 
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius = 0.02f;

    Coroutine jumpForceChange = null;
    Coroutine speedChange = null;

    //Powerup Stuff
    public bool bigMario = false;
    public bool flameMario = false; 

    public int lives
    {
        get => _lives;
        set
        {
            //if (_lives < value) // gained a life
            //if (_lives > value) // lost a life
            _lives = value;

            //if (_lives <= 0 ) // gameover 
            Debug.Log("Lives value has changed to " + _lives.ToString());
        }
    }

    private int _lives = 3;

    public int score
    {
        get => _score;
        set
        {
            _score = value;
            Debug.Log("Lives value has changed to " + _lives.ToString());
        }
    }

    private int _score = 3;
    // Start is called before the first frame update
    void Start()
    {
        //Component References 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //Bad Input Check
        if (speed <= 0) speed = 5.0f;  
        if (jumpForce <= 0) jumpForce = 300.0f;  
        if (groundCheckRadius <= 0) groundCheckRadius = 0.02f;  

        if (!groundCheck) groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        float hInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        if (isGrounded) rb.gravityScale = 1;

        if (curPlayingClips.Length > 0)
        {
            if (Input.GetButtonDown("Fire1") && curPlayingClips[0].clip.name != "fire")
                anim.SetTrigger("fire");
            else if (curPlayingClips[0].clip.name == "fire")
                rb.velocity = Vector2.zero;
            else
            {
                Vector2 movDirection = new Vector2(hInput * speed, rb.velocity.y);
                rb.velocity = movDirection;
            }
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero; 
            rb.AddForce(Vector2.up * jumpForce); 
        } 

        if (!isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.gravityScale = 10;
        }

        //My single frame fire
        bool fire = Input.GetButtonDown("Fire1"); 
        if (flameMario) //Activating fire abilities only if fire flower is obtained
        {
            anim.SetBool("flameMario", flameMario);   
            anim.SetBool("fire", fire);  
        }
        bool smash = Input.GetButtonDown("Jump");

        


        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded); 
        anim.SetBool("smash", smash); //TODO  
        anim.SetBool("bigMario", bigMario);  
        


        if (hInput != 0)
            sr.flipX = (hInput > 0); 
    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 10;
    }
     
    public void StartJumpForceChange()
    {
        Debug.Log("Powerup Pickuped up!");
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
            return;
        }

        StopCoroutine(jumpForceChange); 
        jumpForceChange = null; 
        jumpForce /= 2; 
        StartJumpForceChange();
    }

    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;
        yield return new WaitForSeconds(5.0f);

        jumpForce /= 2;
        jumpForceChange = null;
    }

    public void StartSpeedChange()
    {
        Debug.Log("Powerup Pick up!");
        if (speedChange == null)
        {
            speedChange = StartCoroutine(SpeedChange());
            return;
        }

        StopCoroutine(speedChange);
        speedChange = null;
        speed /= 2;
        StartSpeedChange();
    }

    IEnumerator SpeedChange()
    {
        speed *= 2;
        yield return new WaitForSeconds(5.0f);

        speed /= 2;
        speedChange = null;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherCollider = collision.gameObject;
        Debug.Log("Collision!");
        if (otherCollider.layer == 6)
        {
            Destroy(otherCollider);
        }
    }
}

