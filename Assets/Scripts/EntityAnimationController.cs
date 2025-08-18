using UnityEngine;

public class EntityAnimationController : MonoBehaviour 
{
    private Animator _animator;
    private Entity _entity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _entity = GetComponent<Entity>();
    }

    public void PlayIdle() => _animator.Play("Idle");
    public void PlayRun() => _animator.Play("Run");
    public void PlayJump() => _animator.Play("Jump");
    public void PlayFall() => _animator.Play("Fall");
    public void PlayClimb() => _animator.Play("Climb");
    public void PlayCrouch() => _animator.Play("Crouch");
    public void PlayDeath() => _animator.Play("Death");

    public void UpdateAnimations()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_entity.Rigidbody2D.linearVelocity.x));
        _animator.SetFloat("VerticalVelocity", _entity.Rigidbody2D.linearVelocity.y);
        _animator.SetBool("IsGrounded", _entity.IsGrounded);
        _animator.SetBool("IsClimbing", _entity.IsTouchingLadder);
    }


}