using System;
using UnityEngine;

public  class PlayerAnimationController : MonoBehaviour
{
    [System.Serializable]
    public class AnimationSet
    {
        public AnimationType type;
        public string animationName;
        public float transitionDuration = 0.1f;
    }

    public enum AnimationType
    {
        Idle,
        Run,
        Jump,
        Fall,
        Hurt,
        Crouch,
        Climb
    }

    [SerializeField] private AnimationSet[] _animations;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController _player;

    private AnimationType _currentAnimation;

    private void Start()
    {
        if (_animator == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Компонент Animator не установлен в PlayerAnimationController");
#endif
        }

        if (_player == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Компонент PlayerController не установлен в PlayerAnimationController");
#endif
        }
    }

    public void PlayAnimation(AnimationType type)
    {
        if (_currentAnimation == type)
            return;

        foreach (var animation in _animations)
        {
            if (animation.type == type)
            {
                _animator.CrossFade(animation.animationName, animation.transitionDuration);
                _currentAnimation = type;
                return;
            }
        }

#if UNITY_EDITOR
        Debug.LogWarning($"Анимации {type.ToString()} не найдено!");
#endif
    }

    public void UpdateAnimations()
    {
        // Обновление параметров для blend tree
        _animator.SetFloat("Speed", Mathf.Abs(_player.InputDirection.x));
        _animator.SetBool("IsClimbing", _player.IsTouchingLadder);
    }
}