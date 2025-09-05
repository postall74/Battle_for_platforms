using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] private string _speed = "Speed";

    private Player _player;

    private void Awake()
    {
        TryGetComponent<Player>(out _player);
    }

    public void PlayAnimationRun(float speed)
    {
        _player.Animator.SetFloat(_speed, speed);
    }
}
