using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }

    public float moveSpeed;
    public int facingDirection { get; private set; } = 1; // 1 for right, -1 for left
    public float knockbackDuration;
    public float knockbackSpeed;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FlipSprite()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        facingDirection *= -1;
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void SetVelocity(float speed)
    {
        moveSpeed = speed;
    }

    public void HitKnockback(float knockbackSpeed)
    {
        rb.linearVelocity = new Vector2(facingDirection * -knockbackSpeed, 0);
    }
}
