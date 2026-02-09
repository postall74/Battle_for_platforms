using UnityEngine;

public class EnemyStateContext
{
    public EnemyStateContext(Transform transform, EnemyMovement movement, EnemyAnimator animator, Flipper flipper, GroundChecker groundChecker, IAttacker attacker,
                           Vector2 startPosition, Transform leftPatrolPoint, Transform rightPatrolPoint, float visionRange, float returnThreshold, LayerMask playerLayer)
    {
        Transform = transform;
        Movement = movement;
        Animator = animator;
        Flipper = flipper;
        GroundChecker = groundChecker;
        Attacker = attacker;
        StartPosition = startPosition;
        LeftPatrolPoint = leftPatrolPoint;
        RightPatrolPoint = rightPatrolPoint;
        VisionRange = visionRange;
        ReturnThreshold = returnThreshold;
        PlayerLayer = playerLayer;
    }

    public Transform Transform { get; }
    public EnemyMovement Movement { get; }
    public EnemyAnimator Animator { get; }
    public Flipper Flipper { get; }
    public GroundChecker GroundChecker { get; }
    public IAttacker Attacker { get; }
    public Transform Player { get; set; }
    public Vector2 StartPosition { get; }
    public Transform LeftPatrolPoint { get; }
    public Transform RightPatrolPoint { get; }
    public float VisionRange { get; }
    public float ReturnThreshold { get; }
    public LayerMask PlayerLayer { get; }
}