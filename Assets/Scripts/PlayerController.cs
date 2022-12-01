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
    private bool isFinished = false;
    private bool isLeverArm = false;

    private Rigidbody2D rb;
    private Finish finish;
    private LeverArm leverArm;

    const float speeadXMultiplier = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        leverArm = FindObjectOfType<LeverArm>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("speedX", Mathf.Abs(horizontal));
        if (Input.GetKeyDown(KeyCode.W) && isGround)
        {
            isJump = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFinished) finish.FinishLevel();
            if (isLeverArm) leverArm.ActivateLeverArm();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        LeverArm leverArmTemp = collider.gameObject.GetComponent<LeverArm>();

        if (collider.CompareTag("Finish"))
        {
            isFinished = true;
        }
        if (leverArmTemp != null)
        {
            Debug.Log("Click 'F' ro activate.");
            isLeverArm = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        LeverArm leverArmTemp = collider.gameObject.GetComponent<LeverArm>();

        if (collider.CompareTag("Finish"))
        {
            isFinished = false;
        }
        if (leverArmTemp != null)
        {
            isLeverArm = false;
        }
    }
}
