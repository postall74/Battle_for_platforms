using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;
    public float followSpeed = 3f;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;
    
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        // Автоматически находим игрока если цель не задана
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
        
        // Вычисляем желаемую позицию
        Vector3 desiredPosition = target.position + offset;
        
        // Ограничиваем позицию камеры
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        // Плавно перемещаем камеру
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}