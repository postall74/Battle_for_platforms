using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine), typeof(EnemyMovement), typeof(EnemyAnimation))]
public partial class Enemy : MonoBehaviour
{
    private EnemyStateMachine _stateMachine;
    private EnemyMovement _movement;
    private EnemyAnimation _animation;

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
        _animation = GetComponent<EnemyAnimation>();
    }

    private void SubscribeToEvents()
    {
        _stateMachine.StateChanged += HandleStateChanged;
        _stateMachine.PlayerDetected += HandlePlayerDetected;
        _movement.Movement += _animation.HandleMovement;
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
            _movement.Movement -= _animation.HandleMovement;
        }
    }

    private void HandleStateChanged(EnemyStateType newState)
    {
        switch (newState)
        {
            case EnemyStateType.Patrolling:
            case EnemyStateType.Chasing:
            case EnemyStateType.Returning:
                _animation.HandleMovement(_movement.Speed);
                break;
        }
    }

    private void HandlePlayerDetected(bool isPlayerDetected)
    {
        if(isPlayerDetected)
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