using UnityEngine;
using UnityEngine.InputSystem;

public class Soldier : Player
{
    [Header("Class Specific Settings")]
    [SerializeField] public GameObject arrowPrefab;
    public Transform arrowAttackPoint;
    public float arrowAttackRange = 5f;

    // Arrow Attack Indicator
    public GameObject arrowAttackIndicator;
    private float indicatorDistance;

    // Special Attack Area of Effect
    public float specialAttackRange = 1f;

    // Soldier Stats
    public PlayerStats stats;

    #region PlayerClass Specific States
    public SoldierBaseAttackState baseAttackState { get; private set; }
    public SoldierArrowAttackState arrowAttackState { get; private set; }
    public SoldierArrowChargingState arrowChargingState { get; private set; }
    public SoldierSpecialAttackState specialAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        baseAttackState = new SoldierBaseAttackState(this, stateMachine, "BaseAttack");
        arrowAttackState = new SoldierArrowAttackState(this, stateMachine, "ArrowAttack");
        arrowChargingState = new SoldierArrowChargingState(this, stateMachine, "ArrowAttack");
        specialAttackState = new SoldierSpecialAttackState(this, stateMachine, "SpecialAttack");

        indicatorDistance = arrowAttackIndicator.transform.localPosition.x;
        stats = GetComponent<PlayerStats>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            stats.DecreaseHealth(10);
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        
        stateMachine.ChangeState(baseAttackState);
    }

    private void SecondaryAttack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(arrowAttackState);
    }

    private void SpecialAttack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(specialAttackState);
    }

    private void OnEnable()
    {
        baseAttack.action.started += Attack;
        secondaryAttack.action.started += SecondaryAttack;
        specialAttack.action.started += SpecialAttack;
    }

    private void OnDisable()
    {
        baseAttack.action.started -= Attack;
        secondaryAttack.action.started -= SecondaryAttack;
        specialAttack.action.started -= SpecialAttack;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Draw attack range for arrow attack
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(arrowAttackPoint.position, arrowAttackRange);

        // Draw special attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, specialAttackRange);
    }
    
    public Vector3 FindClosestTargetInArrowRange(Collider2D[] colliders)
    {
        float closestDistance = float.MaxValue;
        Vector3 closestTarget = transform.position + new Vector3(9999999f * facingDirection, 0f, 0f); // this is a default position if no target is found
        
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(arrowAttackPoint.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.transform.position;
                }
            }
        }

        if (closestTarget.x < transform.position.x && facingDirection == 1)
        {
            FlipSprite(); // Update facing direction if target is to the left
        }
        else if (closestTarget.x > transform.position.x && facingDirection == -1)
        {
            FlipSprite(); // Update facing direction if target is to the right
        }   

        return closestTarget;
    }

    public void UpdateArrowIndicator()
    {
        if (arrowAttackIndicator.activeSelf == false) return;

        // Get all colliders in arrow attack range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(arrowAttackPoint.position, arrowAttackRange);
        Vector3 closestTarget = FindClosestTargetInArrowRange(colliders);

        // Calculate direction to target
        Vector3 directionToTarget = (closestTarget - transform.position).normalized;

        // Position indicator on circle around soldier
        Vector3 indicatorPosition = transform.position + (directionToTarget * indicatorDistance);
        arrowAttackIndicator.transform.position = indicatorPosition;

        // Rotate indicator to point toward target
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        angle += facingDirection < 0 ? 180f : 0f; // Adjust angle based on facing direction
        arrowAttackIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
