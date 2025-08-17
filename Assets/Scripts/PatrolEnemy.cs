using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float detectionRange = 3f;
    public Color enemyColor = Color.magenta;
    public Color alertColor = Color.red;
    
    private Vector3 targetPoint;
    private bool movingToB = true;
    private bool isDistracted = false;
    private Transform distractionTarget;
    private float distractionTimer = 0f;
    private float distractionDuration = 2f;
    
    private SpriteRenderer spriteRenderer;
    private PlayerController player;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = enemyColor;
        }
        
        // Устанавливаем начальную цель
        if (pointA != null && pointB != null)
        {
            targetPoint = pointB.position;
        }
        
        // Убеждаемся что коллайдер настроен как триггер
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void Update()
    {
        if (isDistracted)
        {
            HandleDistraction();
        }
        else
        {
            CheckForDistractions();
            Patrol();
        }
    }
    
    void Patrol()
    {
        if (pointA == null || pointB == null) return;
        
        // Движемся к цели
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        
        // Проверяем достижение цели
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            // Переключаем цель
            if (movingToB)
            {
                targetPoint = pointA.position;
                movingToB = false;
            }
            else
            {
                targetPoint = pointB.position;
                movingToB = true;
            }
        }
    }
    
    void CheckForDistractions()
    {
        // Проверяем призраков в радиусе обнаружения
        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        foreach (var ghost in ghosts)
        {
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            if (distance <= detectionRange)
            {
                // Отвлекаемся на призрака
                isDistracted = true;
                distractionTarget = ghost.transform;
                distractionTimer = 0f;
                
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = alertColor;
                }
                return;
            }
        }
    }
    
    void HandleDistraction()
    {
        distractionTimer += Time.deltaTime;
        
        if (distractionTarget != null)
        {
            // Следуем за целью отвлечения
            Vector3 targetPos = distractionTarget.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * 0.8f * Time.deltaTime);
        }
        
        // Возвращаемся к патрулированию через некоторое время
        if (distractionTimer >= distractionDuration || distractionTarget == null)
        {
            isDistracted = false;
            distractionTarget = null;
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = enemyColor;
            }
            
            // Находим ближайшую точку патрулирования
            if (pointA != null && pointB != null)
            {
                float distanceToA = Vector3.Distance(transform.position, pointA.position);
                float distanceToB = Vector3.Distance(transform.position, pointB.position);
                
                if (distanceToA < distanceToB)
                {
                    targetPoint = pointB.position;
                    movingToB = true;
                }
                else
                {
                    targetPoint = pointA.position;
                    movingToB = false;
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerCtrl = other.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.Die();
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Рисуем радиус обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Рисуем линию патрулирования
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}