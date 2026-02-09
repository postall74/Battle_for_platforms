using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [Header("Coin Settings")]
    [SerializeField] private float _respawnTime = 10f;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _seconds;
    private bool _isCollected = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _seconds = new WaitForSeconds(_respawnTime);
    }

    public void Collect()
    {
        if (_isCollected)
            return;

        _isCollected = true;
        _collider.enabled = false;
        _spriteRenderer.enabled = false;

        StartCoroutine(RespawnCoinRoutine());
    }

    private IEnumerator RespawnCoinRoutine()
    {
        yield return _seconds;

        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        _isCollected = false;
    }
}