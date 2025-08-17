using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject targetDoor;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;
    public bool stayPressed = false; // Остается ли кнопка нажатой после активации
    
    private SpriteRenderer spriteRenderer;
    private bool isPressed = false;
    private int objectsOnButton = 0; // Количество объектов на кнопке
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }
    
    public void Activate()
    {
        objectsOnButton++;
        
        if (!isPressed)
        {
            isPressed = true;
            UpdateVisual();
            
            // Открываем дверь
            if (targetDoor != null)
            {
                Door door = targetDoor.GetComponent<Door>();
                if (door != null)
                {
                    door.Open();
                }
            }
        }
    }
    
    public void Stay()
    {
        // Метод вызывается каждый кадр пока объект находится на кнопке
        // Можно использовать для дополнительной логики
    }
    
    public void Deactivate()
    {
        objectsOnButton--;
        
        if (objectsOnButton <= 0 && !stayPressed)
        {
            objectsOnButton = 0;
            isPressed = false;
            UpdateVisual();
            
            // Закрываем дверь
            if (targetDoor != null)
            {
                Door door = targetDoor.GetComponent<Door>();
                if (door != null)
                {
                    door.Close();
                }
            }
        }
    }
    
    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isPressed ? activeColor : inactiveColor;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Stay();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Deactivate();
        }
    }
}