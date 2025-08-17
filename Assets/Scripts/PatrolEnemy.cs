using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float waitTime = 1f;
    public bool canBeDistracted = true;

    [Header("Detection Settings")]
    public float detectionRange = 3f;
    public LayerMask playerLayer;
    public bool ignoreGhosts = false;

    [Header("Visual Settings")]
    public SpriteRenderer enemySprite;
    public Color normalColor = Color.red;
    public Color distractedColor = Color.orange;

    // Состояние врага
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private bool isDistracted = false;
    private Transform distractionTarget = null;
    private float waitTimer = 0f;

    // Компоненты
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (enemySprite != null)
        {
            enemySprite.color = normalColor;
        }

        // Если нет точек патруля, создаем простой патруль
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            CreateSimplePatrol();
        }
    }

    void Update()
    {
        if (isWaiting)
        {
            WaitAtPoint();
        }
        else if (isDistracted && distractionTarget != null)
        {
            ChaseDistraction();
        }
        else
        {
            Patrol();
        }

        // Проверка на игрока и призраков
        CheckForTargets();
    }

    void CreateSimplePatrol()
    {
        // Создаем простой патруль влево-вправо
        patrolPoints = new Transform[2];
        
        GameObject leftPoint = new GameObject("PatrolPoint_Left");
        leftPoint.transform.position = transform.position + Vector3.left * 3f;
        patrolPoints[0] = leftPoint.transform;
        
        GameObject rightPoint = new GameObject("PatrolPoint_Right");
        rightPoint.transform.position = transform.position + Vector3.right * 3f;
        patrolPoints[1] = rightPoint.transform;
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        if (targetPoint == null) return;

        // Двигаемся к точке патруля
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Поворачиваем спрайт
        if (enemySprite != null)
        {
            enemySprite.flipX = direction.x < 0;
        }

        // Проверяем, достигли ли точки
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            StartWaiting();
        }
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = waitTime;
        rb.velocity = Vector2.zero;
    }

    void WaitAtPoint()
    {
        waitTimer -= Time.deltaTime;
        
        if (waitTimer <= 0f)
        {
            isWaiting = false;
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void CheckForTargets()
    {
        if (!canBeDistracted) return;

        // Проверяем на игрока
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (playerCollider != null)
        {
            PlayerController player = playerCollider.GetComponent<PlayerController>();
            if (player != null)
            {
                // Игрок обнаружен - преследуем
                Distract(player.transform);
                return;
            }
        }

        // Проверяем на призраков
        if (!ignoreGhosts)
        {
            Ghost[] ghosts = FindObjectsOfType<Ghost>();
            foreach (Ghost ghost in ghosts)
            {
                if (Vector2.Distance(transform.position, ghost.transform.position) <= detectionRange)
                {
                    // Призрак обнаружен - отвлекаемся
                    Distract(ghost.transform);
                    return;
                }
            }
        }

        // Никого не нашли - возвращаемся к патрулю
        if (isDistracted)
        {
            StopDistraction();
        }
    }

    void Distract(Transform target)
    {
        isDistracted = true;
        distractionTarget = target;
        
        if (enemySprite != null)
        {
            enemySprite.color = distractedColor;
        }
    }

    void StopDistraction()
    {
        isDistracted = false;
        distractionTarget = null;
        
        if (enemySprite != null)
        {
            enemySprite.color = normalColor;
        }
    }

    void ChaseDistraction()
    {
        if (distractionTarget == null)
        {
            StopDistraction();
            return;
        }

        // Двигаемся к цели отвлечения
        Vector2 direction = (distractionTarget.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Поворачиваем спрайт
        if (enemySprite != null)
        {
            enemySprite.flipX = direction.x < 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Если враг касается игрока - убиваем игрока
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Визуализация зоны обнаружения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Визуализация точек патруля
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);
                    
                    // Линии между точками
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                    else if (i == patrolPoints.Length - 1 && patrolPoints[0] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
                    }
                }
            }
        }
    }
}