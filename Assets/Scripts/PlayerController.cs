using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerModelTransform;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private float speeadX = 1f;

    private float _horizontal = 0f;
    private bool _isFacingRight = true;

    private bool _isGround = false;
    private bool _isJump = false;
    private bool _isFinished = false;
    private bool _isLeverArm = false;
    private bool _isDead = false;

    public bool IsDead
    {
        set { _isDead = value; }
    }

    private Rigidbody2D _rb;
    private Finish _finish;
    private LeverArm _leverArm;
    private FixedJoystick _fixedJoystick;

    const float speeadXMultiplier = 50f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
        _fixedJoystick = GameObject.FindGameObjectWithTag("Fixed Joystick").GetComponent<FixedJoystick>(); ;
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _horizontal = _fixedJoystick.Horizontal;
        animator.SetFloat("speedX", Mathf.Abs(_horizontal));

        if (Input.GetKeyDown(KeyCode.W)) Jump();
        if (Input.GetKeyDown(KeyCode.F)) Interact();
    }

    void FixedUpdate()
    {
        if (_isDead) return;
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

    public void Jump()
    {
        if (_isGround && _rb.velocity.y <= 0.2f)
        {
            _isJump = true;
            jumpSound.Play();
        }
    }

    public void Interact()
    {
        if (_isFinished) _finish.FinishLevel();
        if (_isLeverArm) _leverArm.ActivateLeverArm();
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var playerScale = playerModelTransform.localScale;
        playerScale.x *= -1;
        playerModelTransform.localScale = playerScale;
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
