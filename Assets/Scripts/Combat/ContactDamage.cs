using UnityEngine;

namespace BattleForPlatforms.Combat
{
    /// <summary>
    /// Компонент получения урона от контакта.
    /// Используется на объектах, которые могут получать урон при столкновении с атакующими.
    /// </summary>
    public class ContactDamage : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _cooldown = 0.5f;

        private float _lastDamageTime;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var health = collision.gameObject.GetComponent<IHealth>();
            
            if (health == null || !health.IsAlive) return;

            float currentTime = Time.time;
            if (currentTime - _lastDamageTime < _cooldown) return;

            _lastDamageTime = currentTime;
            health.TakeDamage(_damage);
        }
    }
}
