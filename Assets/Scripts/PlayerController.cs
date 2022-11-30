using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speeadX = 1f;
    [SerializeField] private Animator animator;

    private float horizontal = 0f;
    private bool isFacingRight = true;

    private bool isGround = false;
    private bool isJump = false;

    private Rigidbody2D rb;

    const float speeadXMultiplier = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("speedX", Mathf.Abs(horizontal));
        if (Input.GetKeyDown(KeyCode.W) && isGround)
        {
            isJump = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speeadX * speeadXMultiplier * Time.fixedDeltaTime, rb.velocity.y);

        if (isJump) {
            rb.AddForce(new Vector2(0f, 500f));
            isGround = false;
            isJump = false;
        }

        if (horizontal > 0f && !isFacingRight)
        {
            Flip();
        }
        else if(horizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
}
