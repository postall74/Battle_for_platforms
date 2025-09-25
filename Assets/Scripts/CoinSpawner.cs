using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 5f;

    private WaitForSeconds _seconds;

    private void Start()
    {
        StartCoroutine(SpawnCoins());
        _seconds = new WaitForSeconds(_spawnInterval);
    }

    private IEnumerator SpawnCoins()
    {
        while(enabled)
        {
            yield return _seconds;
            SpawnCoinAtRandomPoint();
        }
    }

    private void SpawnCoinAtRandomPoint()
    {
        if (_spawnPoints.Length == 0)
            return;

        int randomIndex = Random.Range (0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[randomIndex];

        Instantiate(_coinPrefab.gameObject, spawnPoint.position, Quaternion.identity);
    }
}