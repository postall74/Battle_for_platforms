public class DeathState : State
{
    public DeathState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayDeath();
        entity.Movement.Move(0); // Останавливаем движение
    }
}