using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, IMovable
{
    protected Rigidbody2D _rigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private int _groundRaysCount = 3;
    [SerializeField] private float _groundRaysSpread = 0.2f;
    [SerializeField] private bool _isFacingRight = true;

    private bool _isGrounded;

    public event Action<bool> GroundedChanged;
    public event Action<float> Movement;
    public event Action Jumped;

    public bool IsGrounded => _isGrounded;
    public float Speed => _speed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool wasGrounded = _isGrounded;
        CheckGrounded();

        if (wasGrounded != _isGrounded)
            GroundedChanged?.Invoke(_isGrounded);
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _groundRaysCount; i++)
        {
            float xOffset = -_groundRaysSpread + (i * _groundRaysSpread);
            Vector2 rayOrigin = _groundCheck.position + new Vector3(xOffset, 0, 0);
            Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * _groundCheckDistance);
        }
    }

    public void Move(float direction)
    {
        Flip(direction);
        _rigidbody.linearVelocity = new Vector2(_speed * direction, _rigidbody.linearVelocity.y);
        Movement?.Invoke(direction);
    }

    public  void Jump()
    {
        if (_isFacingRight == false)
            return;

        _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        Jumped?.Invoke();
    }

    public void Flip(float direction)
    {
        if (direction == 0)
            return;

        bool shouldFaceRight = direction > 0;

        if (shouldFaceRight == _isFacingRight)
            return;

        _isFacingRight = shouldFaceRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public float GetVerticalVelocity()
    {
        return _rigidbody != null ? _rigidbody.linearVelocity.y : 0f;
    }

    private void CheckGrounded()
    {
        _isGrounded = false;

        if (_groundCheck == null)
            return;

        for (int i = 0; i < _groundRaysCount; i++)
        {
            float xOffset = -_groundRaysSpread + (i * _groundRaysSpread);
            Vector2 rayOrigin = _groundCheck.position + new Vector3(xOffset, 0, 0);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, _groundCheckDistance, _groundLayer);

            if (hit.collider != null)
            {
                _isGrounded = true;
                break;
            }
        }
    }
}