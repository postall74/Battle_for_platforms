using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationRun(float speed)
    {
        _animator.SetFloat(PlayerAnimatorData.Speed, Mathf.Abs(speed));
    }
}