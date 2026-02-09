public interface IAttacker
{
    public int Damage { get; }
    public float AttackRange { get; }

    public bool CanAttack();
    public void Attack(IDamageable target);
}