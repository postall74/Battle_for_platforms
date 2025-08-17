using UnityEngine;

public class Door : MonoBehaviour, ITriggerable
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public float openSpeed = 2f;
    public Vector2 openOffset = new Vector2(0, 2f); // Смещение при открытии

    [Header("Visual Settings")]
    public SpriteRenderer doorSprite;
    public Color closedColor = Color.brown;
    public Color openColor = Color.green;

    // Компоненты
    private Vector2 closedPosition;
    private Vector2 openPosition;
    private bool isMoving = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        
        // Устанавливаем начальное состояние
        UpdateVisuals();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveDoor();
        }
    }

    public void Activate()
    {
        if (!isOpen && !isMoving)
        {
            isOpen = true;
            isMoving = true;
            UpdateVisuals();
        }
    }

    public void Deactivate()
    {
        if (isOpen && !isMoving)
        {
            isOpen = false;
            isMoving = true;
            UpdateVisuals();
        }
    }

    void MoveDoor()
    {
        Vector2 targetPosition = isOpen ? openPosition : closedPosition;
        Vector2 currentPosition = transform.position;
        
        float step = openSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Проверяем, достигли ли цели
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }

    void UpdateVisuals()
    {
        if (doorSprite != null)
        {
            Color targetColor = isOpen ? openColor : closedColor;
            doorSprite.color = targetColor;
        }
    }

    public bool IsActivated
    {
        get { return isOpen; }
    }

    void OnDrawGizmos()
    {
        // Визуализация в редакторе
        Gizmos.color = isOpen ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>()?.bounds.size ?? Vector3.one);
        
        // Показываем направление открытия
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + openOffset);
    }
}