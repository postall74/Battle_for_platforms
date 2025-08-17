using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [Header("Falling Rock Settings")]
    public float fallSpeed = 5f;
    public float resetDelay = 3f;
    public bool isFalling = false;
    public bool isReset = true;

    [Header("Trigger Settings")]
    public Transform triggerZone;
    public float triggerDelay = 1f;

    [Header("Visual Settings")]
    public SpriteRenderer rockSprite;
    public Color normalColor = Color.gray;
    public Color fallingColor = Color.red;

    // Компоненты
    private Vector2 startPosition;
    private Rigidbody2D rb;
    private Collider2D rockCollider;
    private bool isTriggered = false;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rockCollider = GetComponent<Collider2D>();

        // Настройка начального состояния
        if (rb != null)
        {
            rb.gravityScale = 0f; // Отключаем гравитацию Unity
        }

        UpdateVisuals();
    }

    void Update()
    {
        if (isFalling)
        {
            Fall();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, может ли объект активировать камень
        if (CanTrigger(other) && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(TriggerFall());
        }
    }

    bool CanTrigger(Collider2D other)
    {
        // Камень может быть активирован игроком или призраком
        return other.CompareTag("Player") || other.GetComponent<Ghost>() != null;
    }

    System.Collections.IEnumerator TriggerFall()
    {
        yield return new WaitForSeconds(triggerDelay);
        
        if (isReset)
        {
            StartFalling();
        }
    }

    void StartFalling()
    {
        isFalling = true;
        isReset = false;
        
        if (rb != null)
        {
            rb.gravityScale = 1f; // Включаем гравитацию
        }

        UpdateVisuals();
    }

    void Fall()
    {
        // Камень падает под действием гравитации
        // Дополнительная логика может быть добавлена здесь
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Когда камень касается земли или другого объекта
        if (isFalling)
        {
            StopFalling();
        }
    }

    void StopFalling()
    {
        isFalling = false;
        
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        UpdateVisuals();

        // Запускаем таймер для сброса
        StartCoroutine(ResetAfterDelay());
    }

    System.Collections.IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        ResetRock();
    }

    void ResetRock()
    {
        transform.position = startPosition;
        isReset = true;
        isTriggered = false;
        
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (rockSprite != null)
        {
            Color targetColor = isFalling ? fallingColor : normalColor;
            rockSprite.color = targetColor;
        }
    }

    void OnDrawGizmos()
    {
        // Визуализация в редакторе
        Gizmos.color = isFalling ? Color.red : Color.gray;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>()?.bounds.size ?? Vector3.one);
        
        // Показываем зону триггера
        if (triggerZone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(triggerZone.position, triggerZone.GetComponent<Collider2D>()?.bounds.size ?? Vector3.one);
        }
    }
}