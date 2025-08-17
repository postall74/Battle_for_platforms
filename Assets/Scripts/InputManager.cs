using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Settings")]
    public bool useWASD = true;
    public bool useArrowKeys = true;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode pauseKey = KeyCode.Escape;

    // Текущие значения ввода
    private float horizontalInput = 0f;
    private bool jumpPressed = false;
    private bool jumpHeld = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        // Сброс значений
        horizontalInput = 0f;
        jumpPressed = false;

        // Горизонтальное движение
        if (useWASD)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput -= 1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput += 1f;
            }
        }
        else if (useArrowKeys)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput -= 1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput += 1f;
            }
        }

        // Ограничиваем значение от -1 до 1
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        // Прыжок
        jumpPressed = Input.GetKeyDown(jumpKey);
        jumpHeld = Input.GetKey(jumpKey);
    }

    // Публичные методы для получения ввода
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    public bool IsJumpPressed()
    {
        return jumpPressed;
    }

    public bool IsJumpHeld()
    {
        return jumpHeld;
    }

    public bool IsPausePressed()
    {
        return Input.GetKeyDown(pauseKey);
    }

    // Методы для проверки конкретных клавиш
    public bool IsKeyPressed(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public bool IsKeyHeld(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public bool IsKeyReleased(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }
}