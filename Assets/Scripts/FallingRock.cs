using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [Header("Rock Settings")]
    public float fallDelay = 2f;
    public float fallSpeed = 5f;
    public float warningTime = 1f;
    
    [Header("Visual")]
    public Color normalColor = Color.gray;
    public Color warningColor = Color.yellow;
    public Color fallingColor = Color.red;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D rockCollider;
    private Vector3 startPosition;
    private bool isFalling = false;
    private bool isWarning = false;
    private float fallTimer = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rockCollider = GetComponent<Collider2D>();
        
        startPosition = transform.position;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
        
        // Set tag
        gameObject.tag = "DeathZone";
    }
    
    void Update()
    {
        if (isWarning)
        {
            fallTimer += Time.deltaTime;
            
            // Flash warning color
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Mathf.Sin(Time.time * 10f) > 0 ? warningColor : normalColor;
            }
            
            if (fallTimer >= warningTime)
            {
                StartFalling();
            }
        }
    }
    
    public void TriggerFall()
    {
        if (!isFalling && !isWarning)
        {
            isWarning = true;
            fallTimer = 0f;
        }
    }
    
    void StartFalling()
    {
        isWarning = false;
        isFalling = true;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = fallingColor;
        }
        
        // Enable physics
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1f;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Rock landed, reset after some time
            StartCoroutine(ResetRock());
        }
    }
    
    IEnumerator ResetRock()
    {
        yield return new WaitForSeconds(3f);
        
        // Reset rock
        transform.position = startPosition;
        isFalling = false;
        isWarning = false;
        fallTimer = 0f;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Can be triggered by player or ghost
        if (other.CompareTag("Player") || other.CompareTag("Ghost"))
        {
            TriggerFall();
        }
    }
}