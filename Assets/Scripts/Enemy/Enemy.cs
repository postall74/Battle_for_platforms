using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine), typeof(EnemyMovement), typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    private EnemyStateMachine _stateMachine;
    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private Flipper _flipper;
    private GroundChecker _groundChecker;

    private void Awake()
    {
        InitializeComponents();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeComponents()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _flipper = GetComponent<Flipper>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void SubscribeToEvents()
    {
        _stateMachine.StateChanged += HandleStateChanged;
        _stateMachine.PlayerDetected += HandlePlayerDetected;
        _movement.Movement += _animator.HandleMovement;
    }

    private void UnsubscribeFromEvents()
    {
        if (_stateMachine != null)
        {
            _stateMachine.StateChanged -= HandleStateChanged;
            _stateMachine.PlayerDetected -= HandlePlayerDetected;
        }

        if (_movement != null)
        {
            _movement.Movement -= _animator.HandleMovement;
        }
    }

    private void HandleStateChanged(EnemyStateType newState)
    {
        switch (newState)
        {
            case EnemyStateType.Patrolling:
            case EnemyStateType.Chasing:
            case EnemyStateType.Returning:
                _animator.HandleMovement(_movement.Speed);
                break;
        }
    }

    private void HandlePlayerDetected(bool isPlayerDetected)
    {
        if (isPlayerDetected)
        {
#if UNITY_EDITOR
            Debug.Log("Enemy detected player!");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Enemy lost player!");
#endif
        }
    }
}