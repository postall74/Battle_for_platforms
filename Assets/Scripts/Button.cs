using UnityEngine;

public class Button : MonoBehaviour, ITriggerable
{
    [Header("Button Settings")]
    public GameObject targetObject; // Объект, который будет активирован
    public bool isActivated = false;
    public bool requiresHold = false; // Нужно ли удерживать кнопку
    public float activationDelay = 0f;

    [Header("Visual Settings")]
    public SpriteRenderer buttonSprite;
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;
    public float activationAnimationSpeed = 2f;

    // Компоненты
    private Collider2D buttonCollider;
    private int activationCount = 0; // Количество объектов на кнопке

    void Start()
    {
        buttonCollider = GetComponent<Collider2D>();
        
        // Настройка коллайдера как триггер
        if (buttonCollider != null)
        {
            buttonCollider.isTrigger = true;
        }

        // Настройка начального цвета
        UpdateVisuals();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, может ли объект активировать кнопку
        if (CanActivate(other))
        {
            activationCount++;
            Activate();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, может ли объект деактивировать кнопку
        if (CanActivate(other))
        {
            activationCount--;
            
            if (requiresHold && activationCount <= 0)
            {
                Deactivate();
            }
        }
    }

    bool CanActivate(Collider2D other)
    {
        // Кнопка может быть активирована игроком или призраком
        return other.CompareTag("Player") || other.GetComponent<Ghost>() != null;
    }

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;
        
        // Активируем целевой объект с задержкой
        if (activationDelay > 0)
        {
            Invoke("ActivateTarget", activationDelay);
        }
        else
        {
            ActivateTarget();
        }

        UpdateVisuals();
    }

    public void Deactivate()
    {
        if (!isActivated) return;

        isActivated = false;
        
        // Деактивируем целевой объект
        DeactivateTarget();
        
        UpdateVisuals();
    }

    void ActivateTarget()
    {
        if (targetObject != null)
        {
            // Если у объекта есть интерфейс ITriggerable
            ITriggerable triggerable = targetObject.GetComponent<ITriggerable>();
            if (triggerable != null)
            {
                triggerable.Activate();
            }
            else
            {
                // Иначе просто активируем GameObject
                targetObject.SetActive(true);
            }
        }
    }

    void DeactivateTarget()
    {
        if (targetObject != null)
        {
            // Если у объекта есть интерфейс ITriggerable
            ITriggerable triggerable = targetObject.GetComponent<ITriggerable>();
            if (triggerable != null)
            {
                triggerable.Deactivate();
            }
            else
            {
                // Иначе просто деактивируем GameObject
                targetObject.SetActive(false);
            }
        }
    }

    void UpdateVisuals()
    {
        if (buttonSprite != null)
        {
            Color targetColor = isActivated ? activeColor : inactiveColor;
            buttonSprite.color = targetColor;
        }
    }

    public bool IsActivated
    {
        get { return isActivated; }
    }

    void OnDrawGizmos()
    {
        // Визуализация в редакторе
        if (targetObject != null)
        {
            Gizmos.color = isActivated ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, targetObject.transform.position);
        }
    }
}