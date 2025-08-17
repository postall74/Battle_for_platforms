using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    [Header("Ghost System")]
    public GameObject ghostPrefab;
    public float recordInterval = 0.1f; // Record every 0.1 seconds
    public int maxGhosts = 5;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float horizontalInput;
    
    // Recording system
    private List<PlayerAction> recordedActions = new List<PlayerAction>();
    private float lastRecordTime;
    private bool isRecording = true;
    
    // Ghost management
    private List<GameObject> activeGhosts = new List<GameObject>();
    
    // Platform standing on ghost
    private bool isStandingOnGhost = false;
    private GameObject currentGhostPlatform;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastRecordTime = Time.time;
        StartRecording();
    }
    
    void Update()
    {
        HandleInput();
        CheckGround();
        RecordActions();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }
    
    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }
    }
    
    void HandleMovement()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
    
    void HandleJump()
    {
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }
    }
    
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Also check if standing on ghost
        if (!isGrounded && isStandingOnGhost && currentGhostPlatform != null)
        {
            // Check if still on top of the ghost
            Collider2D ghostCollider = currentGhostPlatform.GetComponent<Collider2D>();
            if (ghostCollider != null)
            {
                Bounds ghostBounds = ghostCollider.bounds;
                Bounds playerBounds = GetComponent<Collider2D>().bounds;
                
                if (playerBounds.min.y >= ghostBounds.max.y - 0.1f &&
                    playerBounds.min.x < ghostBounds.max.x &&
                    playerBounds.max.x > ghostBounds.min.x)
                {
                    isGrounded = true;
                }
                else
                {
                    isStandingOnGhost = false;
                    currentGhostPlatform = null;
                }
            }
        }
    }
    
    void RecordActions()
    {
        if (!isRecording) return;
        
        if (Time.time - lastRecordTime >= recordInterval)
        {
            PlayerAction action = new PlayerAction
            {
                position = transform.position,
                velocity = rb.velocity,
                isGrounded = isGrounded,
                timestamp = Time.time
            };
            
            recordedActions.Add(action);
            lastRecordTime = Time.time;
        }
    }
    
    public void Die()
    {
        // Create ghost from recorded actions
        if (recordedActions.Count > 0)
        {
            CreateGhost();
        }
        
        // Notify level manager
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.PlayerDied();
        }
        
        // Reset player
        Respawn();
    }
    
    void CreateGhost()
    {
        if (activeGhosts.Count >= maxGhosts)
        {
            // Remove oldest ghost
            if (activeGhosts.Count > 0)
            {
                Destroy(activeGhosts[0]);
                activeGhosts.RemoveAt(0);
            }
        }
        
        // Try to find ghost prefab if not assigned
        if (ghostPrefab == null)
        {
            ghostPrefab = Resources.Load<GameObject>("Ghost");
            if (ghostPrefab == null)
            {
                // Create ghost dynamically if prefab not found
                ghostPrefab = CreateGhostPrefab();
            }
        }
        
        GameObject ghost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
        GhostController ghostController = ghost.GetComponent<GhostController>();
        
        if (ghostController != null)
        {
            ghostController.Initialize(recordedActions.ToArray());
        }
        
        activeGhosts.Add(ghost);
    }
    
    GameObject CreateGhostPrefab()
    {
        GameObject ghost = new GameObject("Ghost");
        ghost.tag = "Ghost";
        
        SpriteRenderer ghostSprite = ghost.AddComponent<SpriteRenderer>();
        ghostSprite.sprite = SpriteGenerator.CreateCircleSprite(new Color(0.5f, 0.5f, 1f, 0.6f), 32);
        ghostSprite.color = new Color(0.5f, 0.5f, 1f, 0.6f);
        
        BoxCollider2D ghostCollider = ghost.AddComponent<BoxCollider2D>();
        ghostCollider.size = new Vector2(1f, 1f);
        
        Rigidbody2D ghostRb = ghost.AddComponent<Rigidbody2D>();
        ghostRb.isKinematic = true;
        
        ghost.AddComponent<GhostController>();
        
        return ghost;
    }
    
    void Respawn()
    {
        // Reset position to start
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        
        // Clear recording and start new
        recordedActions.Clear();
        lastRecordTime = Time.time;
        isRecording = true;
    }
    
    void StartRecording()
    {
        recordedActions.Clear();
        lastRecordTime = Time.time;
        isRecording = true;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if standing on ghost
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // Check if player is on top of ghost
            ContactPoint2D[] contacts = collision.contacts;
            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.normal.y > 0.7f) // Player is on top
                {
                    isStandingOnGhost = true;
                    currentGhostPlatform = collision.gameObject;
                    break;
                }
            }
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            isStandingOnGhost = false;
            currentGhostPlatform = null;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Die();
        }
        else if (other.CompareTag("Finish"))
        {
            Debug.Log("Level Complete!");
            // Add level completion logic here
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