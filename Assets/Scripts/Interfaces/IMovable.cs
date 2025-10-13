public interface IMovable
{
    public bool IsGrounded { get; }
    public float Speed { get; }
    public void Move(float directiond);
    public void Jump();
}