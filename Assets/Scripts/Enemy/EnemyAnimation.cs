using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour, IAnimated
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationRun(float horizontalSpeed)
    {
        _animator.SetFloat(EnemyAnimatorData.HorizontalSpeed, horizontalSpeed);
    }

    public void PlayAnimationDie()
    {
        _animator.SetTrigger(EnemyAnimatorData.IsDie);
    }
}
