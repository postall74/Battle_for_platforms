using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _healthKitPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private int _maxHealthKits = 3;

    private List<GameObject> _spawnedHealthKits = new List<GameObject>();
    private WaitForSeconds _spawnWait;

    private void Start()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
        StartCoroutine(SpawnHealthKitsRoutine());
    }

    private IEnumerator SpawnHealthKitsRoutine()
    {
        while (enabled)
        {
            yield return _spawnWait;
            TrySpawnHealthKit();
        }
    }

    private void TrySpawnHealthKit()
    {
        if (_spawnedHealthKits.Count >= _maxHealthKits || _spawnPoints.Length == 0)
            return;

        _spawnedHealthKits.RemoveAll(kit => kit == null);

        if (_spawnedHealthKits.Count < _maxHealthKits)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            Transform spawnPoint = _spawnPoints[randomIndex];

            GameObject healthKit = Instantiate(_healthKitPrefab, spawnPoint.position, Quaternion.identity);
            _spawnedHealthKits.Add(healthKit);
        }
    }
}