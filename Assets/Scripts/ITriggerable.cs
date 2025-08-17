public interface ITriggerable
{
    void Activate();
    void Deactivate();
    bool IsActivated { get; }
}