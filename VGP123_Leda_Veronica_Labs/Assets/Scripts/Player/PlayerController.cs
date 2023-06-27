using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]



public class PlayerController : MonoBehaviour { 
    //Component references
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    AudioSourceManager asm; 

    //movement variables
    public float speed = 5.0f;
    public float jumpForce = 400.0f;

    //groundcheck stuff
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius = 0.02f;

    //Audio Played for Player 
    public AudioClip jumpSound;
    public AudioClip killSound; 

    Coroutine jumpForceChange = null;
    Coroutine speedChange = null; 



    // Start is called before the first frame update
    void Start() {
        
        //Component References 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        asm = GetComponent<AudioSourceManager>();


        //Bad Input Check
        if (speed <= 0) speed = 5.0f;
        if (jumpForce <= 0) jumpForce = 400.0f;
        if (groundCheckRadius <= 0) groundCheckRadius = 0.02f;

        if (!groundCheck) groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update(){
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
            asm.PlayOneShot(jumpSound, false); 
        }
         
        //My single frame fire
       bool fire = Input.GetButtonDown("Fire1");


        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        
        anim.SetBool("fire", fire);


        if (hInput != 0)
                sr.flipX = (hInput < 0);

    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 1; 
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
        if (collision.CompareTag("DeathZone")){
            //Respawn and lose a life 
            GameManager.Instance.TakeDamage(); 
        } 

        if (collision.CompareTag("Squish"))
        {
            EnemyWalker enemy = collision.gameObject.transform.parent.GetComponent<EnemyWalker>();
            asm.PlayOneShot(killSound, false); 
            enemy.Squish();
            rb.AddForce(Vector2.up * 500);
        }
    }
}

