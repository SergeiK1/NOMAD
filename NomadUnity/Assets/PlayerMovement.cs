using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;


    private enum MovementState { idle, running, jumping, falling }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {

        dirX = Input.GetAxisRaw("Horizontal"); // get axis raw makes it insta stop (delete for graual)
        rb.velocity = new Vector2(dirX*moveSpeed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce); //sets to velocity x so it doesnt reset sideways speed mid jump to 0
        }

        UpdateAnimationState(); // running animation


    }

    private void UpdateAnimationState() 
    {
        MovementState state;
        if(dirX > 0f) // right
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (dirX < 0f) // left
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f) // technically 0 just unity impriceice 
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            // state = MovementState.falling;
        }

        anim.SetInteger("state",(int)state);
    }


    private bool IsGrounded() 
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
