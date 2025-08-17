using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float playbackSpeed = 1f;
    public Color ghostColor = new Color(0.5f, 0.5f, 1f, 0.6f);
    
    private PlayerAction[] recordedActions;
    private int currentActionIndex = 0;
    private float playbackStartTime;
    private bool isPlaying = false;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D ghostCollider;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ghostCollider = GetComponent<Collider2D>();
        
        // Setup ghost appearance
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ghostColor;
        }
        
        // Setup ghost physics
        if (rb != null)
        {
            rb.isKinematic = true; // Ghost doesn't need physics simulation
        }
        
        if (ghostCollider != null)
        {
            ghostCollider.isTrigger = false; // Allow collision for platform functionality
        }
        
        // Set tag
        gameObject.tag = "Ghost";
    }
    
    public void Initialize(PlayerAction[] actions)
    {
        recordedActions = actions;
        playbackStartTime = Time.time;
        isPlaying = true;
        
        if (recordedActions.Length > 0)
        {
            transform.position = recordedActions[0].position;
        }
    }
    
    void Update()
    {
        if (!isPlaying || recordedActions == null || recordedActions.Length == 0)
            return;
        
        PlaybackActions();
    }
    
    void PlaybackActions()
    {
        float currentTime = Time.time - playbackStartTime;
        
        // Find the appropriate action based on time
        for (int i = 0; i < recordedActions.Length; i++)
        {
            float actionTime = recordedActions[i].timestamp - recordedActions[0].timestamp;
            
            if (currentTime >= actionTime && currentTime < actionTime + recordInterval)
            {
                // Apply the recorded action
                ApplyAction(recordedActions[i]);
                currentActionIndex = i;
                break;
            }
        }
        
        // Check if playback is complete
        if (currentTime > recordedActions[recordedActions.Length - 1].timestamp - recordedActions[0].timestamp)
        {
            // Loop the ghost or destroy it
            RestartPlayback();
        }
    }
    
    void ApplyAction(PlayerAction action)
    {
        transform.position = action.position;
        
        // Optional: Apply velocity for visual effect
        if (rb != null)
        {
            rb.velocity = action.velocity;
        }
    }
    
    void RestartPlayback()
    {
        playbackStartTime = Time.time;
        currentActionIndex = 0;
        
        if (recordedActions.Length > 0)
        {
            transform.position = recordedActions[0].position;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ghost can activate buttons and other triggers
        if (other.CompareTag("Button"))
        {
            ButtonController button = other.GetComponent<ButtonController>();
            if (button != null)
            {
                button.Activate();
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // Deactivate buttons when ghost leaves
        if (other.CompareTag("Button"))
        {
            ButtonController button = other.GetComponent<ButtonController>();
            if (button != null)
            {
                button.Deactivate();
            }
        }
    }
    
    // Helper method to get record interval (should match PlayerController)
    private float recordInterval = 0.1f;
}