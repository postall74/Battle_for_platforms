using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private float _respawnTime = 10f;
    [SerializeField] private int _scoreValue = 1;

    private Collider2D _coinCollider;
    private SpriteRenderer _coinRenderer;
    private WaitForSeconds _seconds;
    private bool _isCollected = false;

    public int ScoreValue => _scoreValue;

    private void Awake()
    {
        _coinCollider = GetComponent<Collider2D>();
        _coinRenderer = GetComponent<SpriteRenderer>();
        _seconds = new WaitForSeconds (_respawnTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected == false && collision.TryGetComponent<Player>(out Player player))
            Collect();
    }

    private void Collect()
    {
        _isCollected = true;
        _coinCollider.enabled = false;
        _coinRenderer.enabled = false;
        StartCoroutine(RespawnCoinRoutine());
    }

    private IEnumerator RespawnCoinRoutine()
    {
        yield return _seconds;

        _coinRenderer.enabled = true;
        _coinCollider.enabled = true;
        _isCollected = false;
    }
}