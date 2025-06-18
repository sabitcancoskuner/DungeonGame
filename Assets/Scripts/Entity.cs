using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField] protected float moveSpeed;
    protected int facingDirection = 1; // 1 for right, -1 for left

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void FlipSprite()
    {
        facingDirection *= -1;

    }
}
