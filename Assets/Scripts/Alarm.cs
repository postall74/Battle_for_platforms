using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AlarmTrigger))]
public class Alarm : MonoBehaviour
{
    private const float Mute = 0f;
    private const float MinVolume = 0.01f;   

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

        _volumeCoroutine = StartCoroutine(ChangeVolume(Mute));
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {
        float currentVolume = _audioSource.volume;
        float direction = Mathf.Sign(targetVolume - currentVolume);

        while (Mathf.Abs(_audioSource.volume - targetVolume) > MinVolume)
        {
            _audioSource.volume += direction * _speedVolumeChange * Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = targetVolume;

        if (targetVolume == 0 && _isPlayerInTrigger == false)
            _audioSource.Stop();
    }
}
