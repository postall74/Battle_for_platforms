using UnityEngine;

public class EntityMovementController : MonoBehaviour
{
    [SerializeField] private float _coyoteTime = 0.2f;
    private float _coyoteTimer;

    private Entity _entity;

    private void Awake() => _entity = GetComponent<Entity>();

    public void Move(float direction)
    {
        float speed = _entity.Input.IsCrouching ? _entity.CrouchSpeed : _entity.MoveSpeed;
        _entity.Rigidbody2D.linearVelocity = new Vector2(direction * speed, _entity.Rigidbody2D.linearVelocity.y);

        if (direction != 0 && (direction > 0 != _entity.IsFacingRight))
            _entity.Flip();
    }

    public void Jump()
    {
        _entity.Rigidbody2D.linearVelocity = new Vector2(_entity.Rigidbody2D.linearVelocity.x, _entity.JumpForce);
        _coyoteTimer = 0;
    }

    public void Climb(float vertical)
    {
        _entity.Rigidbody2D.linearVelocity = new Vector2(_entity.Rigidbody2D.linearVelocity.x, vertical * _entity.ClimbSpeed);
        _entity.Rigidbody2D.gravityScale = 0;
    }

    public void Crouch()
    {
        _entity.MainCollider.enabled = false;
        _entity.CrouchCollider.enabled = true;
    }

    public void Stand()
    {
        _entity.MainCollider.enabled = true;
        _entity.CrouchCollider.enabled = false;
    }

    public void ApplyGravity() => 
        _entity.Rigidbody2D.gravityScale = 4;
}