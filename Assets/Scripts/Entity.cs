using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;

    public float moveSpeed;
    public int facingDirection { get; private set; } = 1; // 1 for right, -1 for left

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
}
