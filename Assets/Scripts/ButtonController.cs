using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject targetObject; // Door or platform to control
    public bool isActivated = false;
    public float activationTime = 0.5f; // How long to stay activated
    
    [Header("Visual Feedback")]
    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;
    
    private SpriteRenderer spriteRenderer;
    private float activationTimer = 0f;
    private bool isTimerRunning = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
        
        // Set tag
        gameObject.tag = "Button";
    }
    
    void Update()
    {
        if (isTimerRunning)
        {
            activationTimer -= Time.deltaTime;
            
            if (activationTimer <= 0f)
            {
                Deactivate();
            }
        }
    }
    
    public void Activate()
    {
        isActivated = true;
        activationTimer = activationTime;
        isTimerRunning = true;
        
        UpdateVisual();
        
        // Activate target object
        if (targetObject != null)
        {
            // If it's a door, open it
            DoorController door = targetObject.GetComponent<DoorController>();
            if (door != null)
            {
                door.Open();
            }
            
            // If it's a platform, enable it
            MovingPlatform platform = targetObject.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platform.Activate();
            }
        }
    }
    
    public void Deactivate()
    {
        isActivated = false;
        isTimerRunning = false;
        
        UpdateVisual();
        
        // Deactivate target object
        if (targetObject != null)
        {
            // If it's a door, close it
            DoorController door = targetObject.GetComponent<DoorController>();
            if (door != null)
            {
                door.Close();
            }
            
            // If it's a platform, disable it
            MovingPlatform platform = targetObject.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platform.Deactivate();
            }
        }
    }
    
    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActivated ? activeColor : inactiveColor;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Can be activated by player or ghost
        if (other.CompareTag("Player") || other.CompareTag("Ghost"))
        {
            Activate();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // Only deactivate when player leaves (ghosts will keep it activated)
        if (other.CompareTag("Player"))
        {
            // Don't deactivate immediately, let the timer handle it
        }
    }
}