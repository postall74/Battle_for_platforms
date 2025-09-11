using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public SpriteRenderer SpriteRenderer;
    public Animator Animator;

    private void Awake()
    {
        if (TryGetComponent<Rigidbody2D>(out var rb))
            Rigidbody = rb;

        if (TryGetComponent<SpriteRenderer>(out var sprite))
            SpriteRenderer = sprite;

        if(TryGetComponent<Animator>(out var animator))
            Animator = animator;
    }
}