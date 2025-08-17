using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text instructionText;
    public Text deathCountText;
    
    private int deathCount = 0;
    private PlayerController player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnPlayerDied += OnPlayerDied;
        }
        
        UpdateInstructions();
        UpdateDeathCount();
    }
    
    void UpdateInstructions()
    {
        if (instructionText != null)
        {
            instructionText.text = "WASD/Стрелки - движение\nПробел - прыжок\nR - перезапуск уровня\nEsc - выход\n\nЦель: дойти до зеленого финиша\nПризраки показывают ваши прошлые попытки\nИспользуйте их как платформы и активаторы!";
        }
    }
    
    void OnPlayerDied(System.Collections.Generic.List<PlayerAction> recording)
    {
        deathCount++;
        UpdateDeathCount();
    }
    
    void UpdateDeathCount()
    {
        if (deathCountText != null)
        {
            deathCountText.text = $"Смертей: {deathCount}";
        }
    }
}