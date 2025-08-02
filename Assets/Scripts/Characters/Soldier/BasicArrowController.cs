using UnityEngine;

public class BasicArrowController : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private float arrowLifeTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        arrowLifeTime -= Time.deltaTime;

        if (arrowLifeTime <= 0f)
        {
            DestroyArrow();
        }
    }

    public void RotateArrow(Vector3 targetPos)
    {
        // Rotate the arrow to face the target direction
        Vector3 direction = (targetPos - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.linearVelocity = direction * arrowSpeed; // Set the velocity towards the target
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            arrowSpeed = 0f;

            // Handle collision with enemy
            Debug.Log("Arrow hit an enemy.");
            DestroyArrow();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Handle collision with wall
            Debug.Log("Arrow hit a wall.");
            DestroyArrow();
        }
    }
    
    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
    
}
