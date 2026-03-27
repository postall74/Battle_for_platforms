using UnityEngine;

public class PlayerAnimatorData
{
    public static readonly int HorizontalSpeed = Animator.StringToHash(nameof(HorizontalSpeed));
    public static readonly int VerticalSpeed = Animator.StringToHash(nameof(VerticalSpeed));
    public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
    public static readonly int JumpTrigger = Animator.StringToHash(nameof(JumpTrigger));
    public static readonly int AttackTrigger = Animator.StringToHash(nameof(AttackTrigger));
    public static readonly int DeathTrigger = Animator.StringToHash(nameof(DeathTrigger));
    public static readonly int RespawnTrigger = Animator.StringToHash(nameof(RespawnTrigger));
}