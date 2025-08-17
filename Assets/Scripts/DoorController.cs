using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public float openSpeed = 2f;
    public Vector3 openPosition;
    public Vector3 closedPosition;
    
    [Header("Visual")]
    public Color closedColor = Color.brown;
    public Color openColor = Color.green;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private Vector3 targetPosition;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        
        // Set initial position
        closedPosition = transform.position;
        openPosition = transform.position + Vector3.up * 2f; // Move up when open
        
        targetPosition = isOpen ? openPosition : closedPosition;
        UpdateVisual();
        
        // Set tag
        gameObject.tag = "Door";
    }
    
    void Update()
    {
        // Smoothly move to target position
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
        }
    }
    
    public void Open()
    {
        isOpen = true;
        targetPosition = openPosition;
        
        // Disable collision when open
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        UpdateVisual();
    }
    
    public void Close()
    {
        isOpen = false;
        targetPosition = closedPosition;
        
        // Enable collision when closed
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }
        
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isOpen ? openColor : closedColor;
        }
    }
}