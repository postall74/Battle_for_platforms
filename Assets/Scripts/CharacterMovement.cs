using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] protected float _speed = 5f;
    [SerializeField] protected float _jumpForce = 15f;
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected float _groundCheckDistance = 0.2f;
    [SerializeField] protected int _groundRaysCount = 3;
    [SerializeField] protected float _groundRaysSpread = 0.2f;
    [SerializeField] protected bool _isFacingRight = true;

    protected Rigidbody2D _rigidbody;
    protected bool _isGrounded;

    public event Action<bool> OnGroundedChanged;
    public event Action<float> OnMovement;
    public event Action OnJumped;

    public bool IsGrounded => _isGrounded;
    public float Speed => _speed;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
        {
            _rigidbody.AddComponent<Rigidbody2D>();
        }
    }

    protected virtual void Update()
    {
        bool wasGrounded = _isGrounded;
        CheckGrounded();

        if (wasGrounded != _isGrounded)
            OnGroundedChanged?.Invoke(_isGrounded);
    }

    protected virtual void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < _groundRaysCount; i++)
            {
                float xOffset = -_groundRaysSpread + (i * _groundRaysSpread);
                Vector2 rayOrigin = _groundCheck.position + new Vector3(xOffset, 0, 0);
                Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * _groundCheckDistance);
            }

        }
    }

    public virtual void Move(float direction)
    {
        Flip(direction);
        _rigidbody.linearVelocity = new Vector2(_speed * direction, _rigidbody.linearVelocity.y);
        OnMovement?.Invoke(direction);
    }

    public virtual void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            OnJumped?.Invoke();
        }    
    }

    public virtual void Flip(float direction)
    {
        if (direction == 0)
            return;

        bool shouldFaceRight = direction > 0;

        if (shouldFaceRight != _isFacingRight)
        {
            _isFacingRight = shouldFaceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public float GetVerticalVelocity()
    {
        return _rigidbody.linearVelocity.y;
    }

    protected virtual void CheckGrounded()
    {
        _isGrounded = false;

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