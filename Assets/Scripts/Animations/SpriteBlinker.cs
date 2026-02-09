using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBlinker : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private float _blinkDuration = 0.1f;
    [SerializeField] private int _blinkCount = 3;

    [Header("Effect Colors")]
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private Color _healColor = Color.green;
    [SerializeField] private Color _deathColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _blinkCoroutine;
    private WaitForSeconds _blinkWait;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _blinkWait = new WaitForSeconds(_blinkDuration);
    }

    public void BlinkDamage()
    {
        StartBlinkEffect(_damageColor);
    }

    public void BlinkHeal()
    {
        StartBlinkEffect(_healColor);
    }

    public void BlinkDeath()
    {
        StartBlinkEffect(_deathColor);
    }

    public void StartDeathEffect()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        _blinkCoroutine = StartCoroutine(DeathRoutine());
    }

    private void StartBlinkEffect(Color blinkColor)
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        _blinkCoroutine = StartCoroutine(BlinkRoutine(blinkColor));
    }

    private IEnumerator BlinkRoutine(Color blinkColor)
    {
        for (int i = 0; i < _blinkCount; i++)
        {
            _spriteRenderer.color = blinkColor;
            yield return _blinkWait;
            _spriteRenderer.color = _originalColor;
            yield return _blinkWait;
        }

        _spriteRenderer.color = _originalColor;
    }

    private IEnumerator DeathRoutine()
    {
        for (int i = 0; i < 2; i++)
        {
            _spriteRenderer.color = _deathColor;
            yield return _blinkWait;
            _spriteRenderer.color = _originalColor;
            yield return _blinkWait;
        }

        float fadeDuration = 0.5f;
        float elapsedTime = 0f;
        Color startColor = _originalColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    public void ResetColor()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        _spriteRenderer.color = _originalColor;
        _spriteRenderer.enabled = true;
    }
}