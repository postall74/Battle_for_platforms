using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AlarmTrigger))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _speedVolumeChange = 0.1f;

    private AlarmTrigger _alarmTrigger;
    private bool _isPlayerInTrigger = false;
    private Coroutine _volumeCoroutine;

    private void Awake()
    {
        _alarmTrigger = GetComponent<AlarmTrigger>();
    }

    private void OnEnable()
    {
        _alarmTrigger.PlayerEntered += OnPlayerEntered;
        _alarmTrigger.PlayerExited += OnPlayerExited;
    }

    private void OnDisable()
    {
        _alarmTrigger.PlayerEntered -= OnPlayerEntered;
        _alarmTrigger.PlayerExited -= OnPlayerExited;
    }

    private void OnPlayerEntered()
    {
        _isPlayerInTrigger = true;

        if(_volumeCoroutine != null ) 
            StopCoroutine(_volumeCoroutine);

        _volumeCoroutine = StartCoroutine(ChangeVolume(_maxVolume));

        if (_audioSource.isPlaying == false)
            _audioSource.Play();
    }

    private void OnPlayerExited()
    {
        _isPlayerInTrigger = false;

        if (_volumeCoroutine != null)
            StopCoroutine(_volumeCoroutine);

        _volumeCoroutine = StartCoroutine(ChangeVolume(0f));
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {
        float startVolume = _audioSource.volume;
        float duration = Mathf.Abs(targetVolume - startVolume) / _speedVolumeChange;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _audioSource.volume = Mathf.MoveTowards(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        _audioSource.volume = targetVolume;

        if (targetVolume == 0 && _isPlayerInTrigger == false)
            _audioSource.Stop();
    }
}
