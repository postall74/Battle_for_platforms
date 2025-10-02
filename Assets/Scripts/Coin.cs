using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [Header("Coin Settings")]
    [SerializeField] private float _respawnTime = 10f;
    [SerializeField] private int _scoreValue = 1;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _seconds;
    private bool _isCollected = false;

    public int ScoreValue => _scoreValue;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _seconds = new WaitForSeconds (_respawnTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected)
            return;

        if (_isCollected == false && collision.TryGetComponent<Player>(out Player player))
            Collect();
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