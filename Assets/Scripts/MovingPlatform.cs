using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public bool isActive = false;
    public float moveSpeed = 3f;
    public Vector3 startPosition;
    public Vector3 endPosition;
    
    [Header("Visual")]
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.blue;
    
    private SpriteRenderer spriteRenderer;
    private Vector3 targetPosition;
    private bool movingToEnd = true;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set positions
        startPosition = transform.position;
        endPosition = transform.position + Vector3.right * 4f; // Move right when active
        
        targetPosition = startPosition;
        UpdateVisual();
    }
    
    void Update()
    {
        if (isActive)
        {
            // Move platform
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                // Switch target
                if (movingToEnd)
                {
                    targetPosition = endPosition;
                }
                else
                {
                    targetPosition = startPosition;
                }
                movingToEnd = !movingToEnd;
            }
        }
    }
    
    public void Activate()
    {
        isActive = true;
        UpdateVisual();
    }
    
    public void Deactivate()
    {
        isActive = false;
        // Return to start position
        transform.position = startPosition;
        targetPosition = startPosition;
        movingToEnd = true;
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActive ? activeColor : inactiveColor;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Make player move with platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        // Stop moving with platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}