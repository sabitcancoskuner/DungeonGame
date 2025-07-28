using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soldier : Player
{
    #region PlayerClass Specific States
    public SoldierBaseAttackState baseAttackState { get; private set; }
    public SoldierArrowAttackState arrowAttackState { get; private set; }
    public SoldierArrowChargingState arrowChargingState { get; private set; }
    public SoldierSpecialAttackState specialAttackState { get; private set; }
    #endregion

    public GameObject arrowPrefab;
    public Transform arrowAttackPoint;
    public float arrowAttackRange = 5f;

    // Arrow Attack Indicator
    public GameObject arrowAttackIndicator;
    private float indicatorDistance;

    protected override void Awake()
    {
        base.Awake();

        baseAttackState = new SoldierBaseAttackState(this, stateMachine, "BaseAttack");
        arrowAttackState = new SoldierArrowAttackState(this, stateMachine, "ArrowAttack");
        arrowChargingState = new SoldierArrowChargingState(this, stateMachine, "ArrowAttack");
        specialAttackState = new SoldierSpecialAttackState(this, stateMachine, "SpecialAttack");

        indicatorDistance = arrowAttackIndicator.transform.localPosition.x;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        UpdateArrowIndicator();
    }

    private void Attack(InputAction.CallbackContext context)
    {
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

    private void OnDrawGizmos()
    {
        // Draw attack range for arrow attack
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(arrowAttackPoint.position, arrowAttackRange);
    }
    
    public Vector3 FindClosestTargetInArrowRange(Collider2D[] colliders)
    {
        float closestDistance = float.MaxValue;
        Vector3 closestTarget = transform.position + new Vector3(10f * facingDirection, 0f, 0f); // this is a default position if no target is found
        
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

        return closestTarget;
    }

    private void UpdateArrowIndicator()
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
        arrowAttackIndicator.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); // remove -90
    }
}
