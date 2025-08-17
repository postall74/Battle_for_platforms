using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [Header("Falling Rock Settings")]
    public float fallSpeed = 8f;
    public float triggerDistance = 3f;
    public float resetDelay = 2f;
    public Color rockColor = Color.gray;
    public Color dangerColor = Color.yellow;
    
    private Vector3 startPosition;
    private Vector3 fallTarget;
    private bool isFalling = false;
    private bool hasHitGround = false;
    private float resetTimer = 0f;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D rockCollider;
    private PlayerController player;
    
    void Start()
    {
        startPosition = transform.position;
        fallTarget = startPosition + Vector3.down * 10f; // Падает вниз на 10 единиц
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        rockCollider = GetComponent<Collider2D>();
        player = FindObjectOfType<PlayerController>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = rockColor;
        }
        
        // Делаем коллайдер триггером изначально
        if (rockCollider != null)
        {
            rockCollider.isTrigger = true;
        }
    }
    
    void Update()
    {
        CheckTrigger();
        
        if (isFalling)
        {
            Fall();
        }
        else if (hasHitGround)
        {
            HandleReset();
        }
    }
    
    void CheckTrigger()
    {
        if (isFalling || hasHitGround) return;
        
        // Проверяем игрока
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= triggerDistance && player.transform.position.y <= transform.position.y)
            {
                StartFalling();
                return;
            }
        }
        
        // Проверяем призраков
        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        foreach (var ghost in ghosts)
        {
            float distanceToGhost = Vector2.Distance(transform.position, ghost.transform.position);
            if (distanceToGhost <= triggerDistance && ghost.transform.position.y <= transform.position.y)
            {
                StartFalling();
                return;
            }
        }
    }
    
    void StartFalling()
    {
        isFalling = true;
        
        // Меняем цвет на предупреждающий
        if (spriteRenderer != null)
        {
            spriteRenderer.color = dangerColor;
        }
    }
    
    void Fall()
    {
        transform.position = Vector3.MoveTowards(transform.position, fallTarget, fallSpeed * Time.deltaTime);
        
        // Проверяем столкновение с землей
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            isFalling = false;
            hasHitGround = true;
            resetTimer = 0f;
            
            // Делаем коллайдер твердым
            if (rockCollider != null)
            {
                rockCollider.isTrigger = false;
            }
            
            // Возвращаем нормальный цвет
            if (spriteRenderer != null)
            {
                spriteRenderer.color = rockColor;
            }
        }
    }
    
    void HandleReset()
    {
        resetTimer += Time.deltaTime;
        
        if (resetTimer >= resetDelay)
        {
            Reset();
        }
    }
    
    void Reset()
    {
        transform.position = startPosition;
        isFalling = false;
        hasHitGround = false;
        resetTimer = 0f;
        
        // Возвращаем коллайдер в состояние триггера
        if (rockCollider != null)
        {
            rockCollider.isTrigger = true;
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = rockColor;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Убиваем игрока если он попал под падающий камень
        if (other.CompareTag("Player") && isFalling)
        {
            PlayerController playerCtrl = other.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.Die();
            }
        }
    }
}