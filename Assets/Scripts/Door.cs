using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openHeight = 2f;
    public float moveSpeed = 3f;
    public Color openColor = Color.green;
    public Color closedColor = Color.gray;
    
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
        
        UpdateVisual();
    }
    
    void Update()
    {
        if (isMoving)
        {
            MoveDoor();
        }
    }
    
    public void Open()
    {
        if (!isOpen && !isMoving)
        {
            isOpen = true;
            isMoving = true;
            UpdateVisual();
        }
    }
    
    public void Close()
    {
        if (isOpen && !isMoving)
        {
            isOpen = false;
            isMoving = true;
            UpdateVisual();
        }
    }
    
    void MoveDoor()
    {
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
            
            // Включаем/выключаем коллайдер
            if (doorCollider != null)
            {
                doorCollider.enabled = !isOpen;
            }
        }
    }
    
    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isOpen ? openColor : closedColor;
        }
    }
}