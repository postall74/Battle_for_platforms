using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour, IAnimated
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
}