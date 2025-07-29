using UnityEngine;

public class BasicArrowController : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private float arrowLifeTime = 3f;
    [SerializeField] private Vector3 targetPos;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, arrowSpeed * Time.deltaTime);
        arrowLifeTime -= Time.deltaTime;

        if (arrowLifeTime <= 0f)
        {
            DestroyArrow();
        }
    }

    public void SetTargetPos(Vector3 newTarget)
    {
        targetPos = newTarget;
        RotateArrow();
    }

    private void RotateArrow()
    {
        // Rotate the arrow to face the target direction
        Vector3 direction = (targetPos - transform.position).normalized;

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
            arrowSpeed = 0f;

            // Handle collision with enemy
            Debug.Log("Arrow hit an enemy.");
            animator.SetBool("Pierce", true);
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
