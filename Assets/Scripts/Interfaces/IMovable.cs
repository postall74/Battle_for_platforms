public interface IMovable
{
    public bool IsGrounded { get; }
    public float Speed { get; }
    public void Move(float direction);
    public void Jump();
}