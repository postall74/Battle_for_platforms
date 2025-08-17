using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public LayerMask groundLayer = 1;
    
    [Header("Recording")]
    public float recordingRate = 10f; // записей в секунду
    
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isGrounded;
    private float horizontalInput;
    
    // Система записи действий
    private List<PlayerAction> currentRecording = new List<PlayerAction>();
    private float recordingTimer;
    private Vector3 startPosition;
    
    // События
    public System.Action<List<PlayerAction>> OnPlayerDied;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        startPosition = transform.position;
        
        // Начинаем запись
        StartRecording();
    }
    
    void Update()
    {
        HandleInput();
        CheckGrounded();
        RecordActions();
    }
    
    void FixedUpdate()
    {
        Move();
    }
    
    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    
    void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
    
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    
    void CheckGrounded()
    {
        // Проверяем землю под игроком
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 
            col.bounds.extents.y + 0.1f, groundLayer);
        isGrounded = hit.collider != null;
        
        // Также проверяем призраков под игроком
        if (!isGrounded)
        {
            Collider2D[] ghosts = Physics2D.OverlapBoxAll(
                transform.position + Vector3.down * (col.bounds.extents.y + 0.05f),
                new Vector2(col.bounds.size.x * 0.9f, 0.1f), 0f);
                
            foreach (var ghost in ghosts)
            {
                if (ghost.CompareTag("Ghost"))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }
    
    void RecordActions()
    {
        recordingTimer += Time.deltaTime;
        
        if (recordingTimer >= 1f / recordingRate)
        {
            PlayerAction action = new PlayerAction
            {
                position = transform.position,
                velocity = rb.velocity,
                isGrounded = this.isGrounded,
                timestamp = Time.time
            };
            
            currentRecording.Add(action);
            recordingTimer = 0f;
        }
    }
    
    public void Die()
    {
        // Передаем запись в систему призраков
        OnPlayerDied?.Invoke(new List<PlayerAction>(currentRecording));
        
        // Респавн
        Respawn();
    }
    
    void Respawn()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
        
        // Начинаем новую запись
        StartRecording();
    }
    
    void StartRecording()
    {
        currentRecording.Clear();
        recordingTimer = 0f;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathTrap"))
        {
            Die();
        }
        else if (other.CompareTag("Finish"))
        {
            Debug.Log("Level Complete!");
            // Здесь можно добавить логику завершения уровня
        }
    }
}

[System.Serializable]
public class PlayerAction
{
    public Vector3 position;
    public Vector2 velocity;
    public bool isGrounded;
    public float timestamp;
}