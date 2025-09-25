public interface IMovable
{
    public bool IsGrounded { get; }
    public void Move(float speed);
    public void Jump();
    public void Flip(float direction);
}