using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 2, -10);
    public bool followX = true;
    public bool followY = true;

    [Header("Bounds")]
    public bool useBounds = false;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void Start()
    {
        // Если цель не назначена, ищем игрока
        if (target == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;
        
        // Ограничиваем движение по осям
        if (!followX)
        {
            desiredPosition.x = transform.position.x;
        }
        if (!followY)
        {
            desiredPosition.y = transform.position.y;
        }

        // Применяем границы
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Плавно перемещаем камеру к цели
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            // Визуализация границ камеры
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0), 
                               new Vector3(maxX - minX, maxY - minY, 1));
        }
    }
}