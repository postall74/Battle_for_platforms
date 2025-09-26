using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] protected float _speed = 5f;
    [SerializeField] protected float _jumpForce = 15f;
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected float _groundCheckDistance = 0.2f;
    [SerializeField] protected bool _isFacingRight = true;

    protected Rigidbody2D _rigidbody;
    protected bool _isGrounded;

    public bool IsGrounded => _isGrounded;
    public float Speed => _speed;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        CheckGrounded();
    }

    protected virtual void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _groundCheckDistance);
        }
    }

    public virtual void Move(float direction)
    {
        Flip(direction);
        _rigidbody.linearVelocity = new Vector2(_speed * direction, _rigidbody.linearVelocity.y);
    }

    public virtual void Jump()
    {
        if (_isGrounded)
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
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

    protected virtual void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);
        _isGrounded = hit.collider != null;
    }

    public float GetVerticalVelocity()
    {
        return _rigidbody.linearVelocity.y;
    }
}