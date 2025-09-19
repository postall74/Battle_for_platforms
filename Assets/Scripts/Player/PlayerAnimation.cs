using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour, IAnimated
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationRun(float horizontalSpeed)
    {
        _animator.SetFloat(PlayerAnimatorData.HorizontalSpeed, Mathf.Abs(horizontalSpeed));
    }

    public void PlayAnimationJumpOrFall(float verticalVelocity, bool isGrounded)
    {
        _animator.SetFloat(PlayerAnimatorData.VerticalSpeed, verticalVelocity);
        _animator.SetBool(PlayerAnimatorData.IsGrounded, isGrounded);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(PlayerAnimatorData.JumpTrigger);
    }

    public void ResetJumpTrigger()
    {
        _animator.ResetTrigger(PlayerAnimatorData.JumpTrigger);
    }

    public void PlayAnimationDie()
    {
        _animator.SetBool(PlayerAnimatorData.IsDie, true);
    }
}