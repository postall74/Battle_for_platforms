using System.Collections;
using UnityEngine;

public class EntityFXController : MonoBehaviour
{
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private int _blinkCount = 5;

    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private Coroutine _blinkRoutine;
    private WaitForSeconds _waitTimeDuration;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        _waitTimeDuration = new WaitForSeconds(_blinkDuration / (_blinkCount * 2));
    }

    public void StartBlink()
    {
        if (_blinkRoutine != null) StopCoroutine(_blinkRoutine);
        _blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        Material blinkMaterial = new Material(Shader.Find("Sprites/Default"));

        for (int i = 0; i < _blinkCount; i++)
        {
            _spriteRenderer.material = blinkMaterial;
            yield return _waitTimeDuration;
            _spriteRenderer.material = _originalMaterial;
            yield return _waitTimeDuration;
        }
    }
}