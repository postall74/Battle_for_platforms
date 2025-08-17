using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private List<PlayerAction> recording;
    private int currentActionIndex = 0;
    private float playbackTimer = 0f;
    private bool isPlaying = false;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D ghostCollider;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghostCollider = GetComponent<Collider2D>();
        
        // Убеждаемся что коллайдер настроен правильно
        if (ghostCollider != null)
        {
            ghostCollider.isTrigger = true;
        }
    }
    
    public void Initialize(List<PlayerAction> playerRecording)
    {
        recording = new List<PlayerAction>(playerRecording);
        currentActionIndex = 0;
        playbackTimer = 0f;
        isPlaying = true;
        
        if (recording.Count > 0)
        {
            transform.position = recording[0].position;
        }
    }
    
    void Update()
    {
        if (!isPlaying || recording == null || recording.Count == 0)
            return;
            
        PlaybackRecording();
    }
    
    void PlaybackRecording()
    {
        if (currentActionIndex >= recording.Count)
        {
            // Запись закончилась - останавливаем воспроизведение
            isPlaying = false;
            return;
        }
        
        PlayerAction currentAction = recording[currentActionIndex];
        
        // Плавно перемещаем призрака к следующей позиции
        if (currentActionIndex < recording.Count - 1)
        {
            PlayerAction nextAction = recording[currentActionIndex + 1];
            float actionDuration = nextAction.timestamp - currentAction.timestamp;
            
            if (actionDuration > 0)
            {
                playbackTimer += Time.deltaTime;
                float t = playbackTimer / actionDuration;
                
                // Интерполируем позицию
                transform.position = Vector3.Lerp(currentAction.position, nextAction.position, t);
                
                // Переходим к следующему действию
                if (t >= 1f)
                {
                    currentActionIndex++;
                    playbackTimer = 0f;
                }
            }
            else
            {
                // Если временной интервал нулевой, сразу переходим к следующему действию
                currentActionIndex++;
                transform.position = currentAction.position;
            }
        }
        else
        {
            // Последнее действие
            transform.position = currentAction.position;
            isPlaying = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Призрак может активировать триггеры (кнопки, двери)
        if (other.CompareTag("Button"))
        {
            ButtonTrigger button = other.GetComponent<ButtonTrigger>();
            if (button != null)
            {
                button.Activate();
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        // Поддерживаем активацию кнопок пока призрак находится на них
        if (other.CompareTag("Button"))
        {
            ButtonTrigger button = other.GetComponent<ButtonTrigger>();
            if (button != null)
            {
                button.Stay();
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // Деактивируем кнопки когда призрак уходит
        if (other.CompareTag("Button"))
        {
            ButtonTrigger button = other.GetComponent<ButtonTrigger>();
            if (button != null)
            {
                button.Deactivate();
            }
        }
    }
    
    // Проверяем можно ли стоять на этом призраке
    public bool CanStandOn()
    {
        return isPlaying || currentActionIndex < recording.Count;
    }
}