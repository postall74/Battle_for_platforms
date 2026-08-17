using UnityEngine;

namespace BattleForPlatforms.Player
{
    /// <summary>
    /// Данные для анимации игрока.
    /// Содержит хеши параметров аниматора для оптимизации.
    /// </summary>
    public class PlayerAnimatorData
    {
        public readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        public readonly int InAirHash = Animator.StringToHash("InAir");
        public readonly int HitHash = Animator.StringToHash("Hit");
        public readonly int DeadHash = Animator.StringToHash("Dead");
    }
}
