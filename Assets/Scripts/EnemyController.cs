using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float timeToWait = 5f;
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private Transform enemyModelTransform;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundaryPosition;

    private bool _isFacingRight = true;
    private bool _isWait = false;
    private bool _isChasingPlayer = false;
    private bool _collidedWithPlayer = false;

    private float _walkSpeed;
    private float _waitTime;
    private float _chaseTime;

    public bool IsFacingRight
    {
        get => _isFacingRight;
    }

    public void StartChasingPlayer()
    {
        _isChasingPlayer = true;
        _chaseTime = timeToChase;
        _walkSpeed = chasingSpeed;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
        _chaseTime = timeToChase;
        _walkSpeed = patrolSpeed;
    }

    private void Update()
    {
        if (_isWait && !_isChasingPlayer) StartWaitTimer();
        if (_isChasingPlayer) StartChasingTimer();

        if (ShouldWait()) _isWait = true;
    }

    private void FixedUpdate()
    {
        Vector2 nextPoint = Vector2.right * _walkSpeed * Time.fixedDeltaTime;

        if(_isChasingPlayer && _collidedWithPlayer) return;

        if (!_isWait && !_isChasingPlayer) Patrol(nextPoint);
        if (_isChasingPlayer) ChasePlayer(nextPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision) => HitPlayer(collision, true);

    private void OnCollisionExit2D(Collision2D collision) => HitPlayer(collision, false);

    private void Patrol(Vector2 nextPoint)
    {
        if (!_isFacingRight) nextPoint.x *= -1;
        _rb.MovePosition((Vector2)transform.position + nextPoint);
    }

    private void ChasePlayer(Vector2 nextPoint)
    {
        float disatance = _playerTransform.position.x - transform.position.x;
        if (disatance < 0) nextPoint.x *= -1;

        if (disatance > 0.2f && !_isFacingRight) Flip();
        else if (disatance < 0.2f && _isFacingRight) Flip();

        _rb.MovePosition((Vector2)transform.position + nextPoint);
    }

    private bool ShouldWait()
    {
        bool isOutOfRightBoundary = _isFacingRight && transform.position.x >= _rightBoundaryPosition.x;
        bool isOutOfLeftBoundary = !_isFacingRight && transform.position.x <= _leftBoundaryPosition.x;

        return isOutOfRightBoundary || isOutOfLeftBoundary;
    }

    private void StartWaitTimer()
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = timeToWait;
            _isWait = false;
            Flip();
        }
    }

    private void StartChasingTimer()
    {
        _chaseTime -= Time.deltaTime;

        if (_chaseTime <0f)
        {
            _isChasingPlayer = false;
            _chaseTime = timeToChase;
            _walkSpeed = patrolSpeed;
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var playerScale = enemyModelTransform.localScale;
        playerScale.x *= -1;
        enemyModelTransform.localScale = playerScale;
    }

    private void HitPlayer(Collision2D collision, bool hit)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) _collidedWithPlayer = hit;
    }

}
