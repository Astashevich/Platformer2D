using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speeadX = 1f;
    [SerializeField] private Animator animator;

    private float _horizontal = 0f;
    private bool _isFacingRight = true;

    private bool _isGround = false;
    private bool _isJump = false;
    private bool _isFinished = false;
    private bool _isLeverArm = false;

    private Rigidbody2D _rb;
    private Finish _finish;
    private LeverArm _leverArm;

    const float speeadXMultiplier = 50f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("speedX", Mathf.Abs(_horizontal));
        if (Input.GetKeyDown(KeyCode.W) && _isGround)
        {
            _isJump = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFinished) _finish.FinishLevel();
            if (_isLeverArm) _leverArm.ActivateLeverArm();
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * speeadX * speeadXMultiplier * Time.fixedDeltaTime, _rb.velocity.y);

        if (_isJump) {
            _rb.AddForce(new Vector2(0f, 500f));
            _isGround = false;
            _isJump = false;
        }

        if (_horizontal > 0f && !_isFacingRight)
        {
            Flip();
        }
        else if(_horizontal < 0 && _isFacingRight)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        LeverArm leverArmTemp = collider.gameObject.GetComponent<LeverArm>();

        if (collider.CompareTag("Finish"))
        {
            _isFinished = true;
        }
        if (leverArmTemp != null)
        {
            Debug.Log("Click 'F' ro activate.");
            _isLeverArm = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        LeverArm leverArmTemp = collider.gameObject.GetComponent<LeverArm>();

        if (collider.CompareTag("Finish"))
        {
            _isFinished = false;
        }
        if (leverArmTemp != null)
        {
            _isLeverArm = false;
        }
    }
}
