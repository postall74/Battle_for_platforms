using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    [Header("Recording Settings")]
    public float recordInterval = 0.1f; // Запись каждые 0.1 секунды
    public int maxRecordedFrames = 300; // Максимум 30 секунд записи при 10 FPS

    // Компоненты
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Состояние игрока
    private bool isGrounded;
    private bool isDead = false;
    private float horizontalInput;

    // Система записи
    private List<PlayerAction> recordedActions = new List<PlayerAction>();
    private float lastRecordTime;
    private bool isRecording = true;

    // Ссылка на GameManager
    private GameManager gameManager;

    [System.Serializable]
    public class PlayerAction
    {
        public Vector2 position;
        public bool isMoving;
        public bool isJumping;
        public float horizontalInput;
        public float timestamp;

        public PlayerAction(Vector2 pos, bool moving, bool jumping, float input, float time)
        {
            position = pos;
            isMoving = moving;
            isJumping = jumping;
            horizontalInput = input;
            timestamp = time;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();

        // Начинаем запись
        StartRecording();
    }

    void Update()
    {
        if (isDead) return;

        // Получаем ввод через InputManager
        if (InputManager.Instance != null)
        {
            horizontalInput = InputManager.Instance.GetHorizontalInput();
            
            // Прыжок
            if (InputManager.Instance.IsJumpPressed() && isGrounded)
            {
                Jump();
            }
        }
        else
        {
            // Fallback к стандартному вводу
            horizontalInput = Input.GetAxisRaw("Horizontal");
            
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
        }

        // Проверка земли
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Запись действий
        if (isRecording && Time.time - lastRecordTime >= recordInterval)
        {
            RecordAction();
            lastRecordTime = Time.time;
        }

        // Обновление анимации
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Движение
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Поворот спрайта
        if (horizontalInput != 0)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", Mathf.Abs(horizontalInput) > 0.1f);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.velocity.y);
        }
    }

    void RecordAction()
    {
        PlayerAction action = new PlayerAction(
            transform.position,
            Mathf.Abs(horizontalInput) > 0.1f,
            !isGrounded && rb.velocity.y > 0,
            horizontalInput,
            Time.time
        );

        recordedActions.Add(action);

        // Ограничиваем количество записанных кадров
        if (recordedActions.Count > maxRecordedFrames)
        {
            recordedActions.RemoveAt(0);
        }
    }

    public void StartRecording()
    {
        isRecording = true;
        recordedActions.Clear();
        lastRecordTime = Time.time;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(recordedActions);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        StopRecording();

        // Создаем призрака
        if (gameManager != null)
        {
            gameManager.CreateGhost(GetRecordedActions());
        }

        // Скрываем игрока
        spriteRenderer.enabled = false;
        rb.simulated = false;

        // Перезапускаем через небольшую задержку
        StartCoroutine(RespawnAfterDelay(0.5f));
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    public void Respawn()
    {
        isDead = false;
        spriteRenderer.enabled = true;
        rb.simulated = true;
        
        // Сбрасываем позицию
        if (gameManager != null)
        {
            transform.position = gameManager.GetSpawnPoint();
        }

        // Начинаем новую запись
        StartRecording();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}