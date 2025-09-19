using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour, IMovable
{
    [SerializeField] private float _speedX = 5f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    private Rigidbody2D _rigidbody;
    private Quaternion _orginalRotation;
    private readonly float _flipAngle = 180f;
    private Quaternion _flipRotation;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _orginalRotation = transform.rotation;
        _flipRotation = _orginalRotation * Quaternion.Euler(0, _flipAngle, 0);
    }

    private void Update()
    {
        CheckGrounded();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _groundCheckDistance);
    }

    public void Move(float direction)
    {
        Flip(direction);
        _rigidbody.linearVelocity = new Vector2(_speedX * direction, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        if (_isGrounded)
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public float GetVerticalVelocity()
    {
        return _rigidbody.linearVelocity.y;
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);
        _isGrounded = hit.collider != null;
    }

    private void Flip(float direction)
    {
        if (direction != 0)
            transform.rotation = direction < 0 ? _flipRotation : _orginalRotation;
    }
}
