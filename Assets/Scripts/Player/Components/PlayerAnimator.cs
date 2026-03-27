using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour, IAnimated
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void HandleMovement(float horizontalSpeed)
    {
        _animator.SetFloat(PlayerAnimatorData.HorizontalSpeed, Mathf.Abs(horizontalSpeed));
    }

    public void HandleGroundedChanged(bool isGrounded)
    {
        _animator.SetBool(PlayerAnimatorData.IsGrounded, isGrounded);
    }

    public void HandleVerticalVelocity(float verticalVelocity)
    {
        _animator.SetFloat(PlayerAnimatorData.VerticalSpeed, verticalVelocity);
    }

    public void HandleJump()
    {
        _animator.SetTrigger(PlayerAnimatorData.JumpTrigger);
    }

    public void HandleAttack()
    {
        _animator.SetTrigger(PlayerAnimatorData.AttackTrigger);
    }

    public void HandleDeath()
    {
        _animator.SetTrigger(PlayerAnimatorData.DeathTrigger);
    }

    public void HandleRespawn()
    {
        _animator.SetTrigger(PlayerAnimatorData.RespawnTrigger);
    }
}