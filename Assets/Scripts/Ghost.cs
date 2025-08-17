using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float playbackSpeed = 1f;
    public Color ghostColor = new Color(0.5f, 0.5f, 1f, 0.6f);
    public bool canActivateTriggers = true;
    public bool canBePlatform = true;

    // Компоненты
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D ghostCollider;

    // Воспроизведение записи
    private List<PlayerController.PlayerAction> recordedActions;
    private int currentActionIndex = 0;
    private float playbackStartTime;
    private bool isPlaying = false;

    // Физика для платформы
    private bool isPlayerStanding = false;
    private Transform playerTransform;
    private float platformTolerance = 0.1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ghostCollider = GetComponent<Collider2D>();

        // Настройка внешнего вида призрака
        SetupGhostAppearance();

        // Настройка коллайдера как триггер
        if (ghostCollider != null)
        {
            ghostCollider.isTrigger = true;
        }
    }

    void SetupGhostAppearance()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ghostColor;
        }
    }

    public void StartPlayback(List<PlayerController.PlayerAction> actions)
    {
        recordedActions = new List<PlayerController.PlayerAction>(actions);
        currentActionIndex = 0;
        playbackStartTime = Time.time;
        isPlaying = true;

        // Устанавливаем начальную позицию
        if (recordedActions.Count > 0)
        {
            transform.position = recordedActions[0].position;
        }
    }

    void Update()
    {
        if (!isPlaying || recordedActions == null || recordedActions.Count == 0) return;

        // Воспроизводим запись
        PlaybackRecording();

        // Обновляем анимацию
        UpdateAnimation();
    }

    void PlaybackRecording()
    {
        if (currentActionIndex >= recordedActions.Count)
        {
            // Запись закончилась
            isPlaying = false;
            return;
        }

        PlayerController.PlayerAction currentAction = recordedActions[currentActionIndex];
        float expectedTime = currentAction.timestamp - recordedActions[0].timestamp;
        float actualTime = (Time.time - playbackStartTime) * playbackSpeed;

        // Если пришло время для следующего действия
        if (actualTime >= expectedTime)
        {
            // Устанавливаем позицию
            transform.position = currentAction.position;

            // Поворачиваем спрайт
            if (currentAction.horizontalInput != 0)
            {
                spriteRenderer.flipX = currentAction.horizontalInput < 0;
            }

            currentActionIndex++;
        }
    }

    void UpdateAnimation()
    {
        if (currentActionIndex < recordedActions.Count)
        {
            PlayerController.PlayerAction currentAction = recordedActions[currentActionIndex];
            
            // Здесь можно добавить анимацию призрака
            // Например, изменение прозрачности при движении
            if (currentAction.isMoving)
            {
                Color currentColor = spriteRenderer.color;
                currentColor.a = 0.8f;
                spriteRenderer.color = currentColor;
            }
            else
            {
                Color currentColor = spriteRenderer.color;
                currentColor.a = 0.6f;
                spriteRenderer.color = currentColor;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Активация триггеров (кнопки, двери и т.д.)
        if (canActivateTriggers)
        {
            ITriggerable triggerable = other.GetComponent<ITriggerable>();
            if (triggerable != null)
            {
                triggerable.Activate();
            }
        }

        // Проверка, стоит ли игрок на призраке
        if (canBePlatform && other.CompareTag("Player"))
        {
            CheckIfPlayerStanding(other.transform);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Постоянная проверка, стоит ли игрок на призраке
        if (canBePlatform && other.CompareTag("Player"))
        {
            CheckIfPlayerStanding(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerStanding = false;
            playerTransform = null;
        }
    }

    void CheckIfPlayerStanding(Transform player)
    {
        // Проверяем, находится ли игрок сверху призрака
        float playerBottom = player.position.y - player.GetComponent<Collider2D>().bounds.extents.y;
        float ghostTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        if (playerBottom >= ghostTop - platformTolerance)
        {
            isPlayerStanding = true;
            playerTransform = player;
        }
    }

    void FixedUpdate()
    {
        // Если игрок стоит на призраке, поддерживаем его
        if (isPlayerStanding && playerTransform != null)
        {
            SupportPlayer();
        }
    }

    void SupportPlayer()
    {
        Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Проверяем, что игрок все еще сверху
            float playerBottom = playerTransform.position.y - playerTransform.GetComponent<Collider2D>().bounds.extents.y;
            float ghostTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

            if (playerBottom >= ghostTop - platformTolerance)
            {
                // Если игрок падает, останавливаем его падение
                if (playerRb.velocity.y < 0)
                {
                    Vector3 newPosition = playerTransform.position;
                    newPosition.y = ghostTop + playerTransform.GetComponent<Collider2D>().bounds.extents.y;
                    playerTransform.position = newPosition;
                    
                    Vector2 newVelocity = playerRb.velocity;
                    newVelocity.y = 0;
                    playerRb.velocity = newVelocity;
                }
            }
            else
            {
                isPlayerStanding = false;
                playerTransform = null;
            }
        }
    }

    public void DestroyGhost()
    {
        Destroy(gameObject);
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = speed;
    }
}