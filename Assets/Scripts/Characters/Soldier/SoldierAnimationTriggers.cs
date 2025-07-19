using UnityEngine;

public class SoldierAnimationTriggers : MonoBehaviour, IPlayerAnimationTriggers
{
    private Soldier soldier;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
    }

    public void BaseAttackAnimationFinishTrigger()
    {
        soldier.stateMachine.currentState.BaseAttackAnimationFinishTrigger();
    }

    public void SecondaryAttackAnimationFinishTrigger()
    {
        soldier.stateMachine.currentState.SecondaryAttackAnimationFinishTrigger();
    }

    public void SpecialAttackAnimationFinishTrigger()
    {
        soldier.stateMachine.currentState.SpecialAttackAnimationFinishTrigger();
    }

    public void BaseAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(soldier.attackPoint.position, soldier.attackRange);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // change this to enemy
            {
                Debug.Log("Hit an enemy.");
            }
        }
    }

    private void ArrowAttackTrigger()
    {
        // instantiate arrow
        Debug.Log("Arrow attack triggered.");
    }

    private void ArrowChargeTrigger()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Arrow charge triggered.");
            soldier.animator.speed = 0;
        }
    }

    private void SpecialAttackTrigger()
    {
        // Handle special attack logic here
        Debug.Log("Special attack triggered.");
    }

}
