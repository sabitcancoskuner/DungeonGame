using UnityEngine;

public class BasicArrowController : MonoBehaviour
{
    public float arrowSpeed = 10f;
    public Vector3 target;


    private void Start()
    {
        RotateArrow();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, arrowSpeed * Time.deltaTime);
    }

    private void RotateArrow()
    {
        // Rotate the arrow to face the target direction
        Vector3 direction = (target - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            // Handle collision with enemy
            Debug.Log("Arrow hit an enemy.");
        }

        Destroy(gameObject); // Destroy the arrow on hit
    }
    
}
