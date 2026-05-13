using UnityEngine;

public class ArmoredOrcAnimationTriggers : MonoBehaviour, IEnemyAnimationTriggers
{
    private ArmoredOrc armoredOrc;

    private void Awake()
    {
        armoredOrc = GetComponentInParent<ArmoredOrc>();
    }

    public void BaseAttackAnimationFinishTrigger()
    {
        // Logic for when the base attack animation finishes
        armoredOrc.stateMachine.currentState.BaseAttackAnimationFinishTrigger();
    }

    public void SecondaryAttackAnimationFinishTrigger()
    {
        // Logic for when the secondary attack animation finishes
        armoredOrc.stateMachine.currentState.SecondaryAttackAnimationFinishTrigger();

    }

    public void BaseAttackTrigger()
    {
        Vector2 attackPoint = armoredOrc.baseAttackPoint.position;

        // Use OverlapCircleAll to detect all colliders within the base attack range, because the base attack is a circular area
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint, armoredOrc.baseAttackRange);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Debug.Log("Armored Orc hit the player with base attack.");
                // Additional logic for hitting the player can be added here
            }
        }

    }
    
    public void PierceAttackTrigger()
    {
        Vector2 attackDirection = new Vector2(armoredOrc.facingDirection, 0); // Horizontal line based on facing direction
        Vector2 startPosition = armoredOrc.pierceAttackPoint.position;
        
        // Use RaycastAll to detect all colliders along the pierce attack line, because the pierce attack is a straight line
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, attackDirection, armoredOrc.pierceAttackRange);
        
        foreach (var hit in hits)
        {
            if (hit.collider.GetComponent<Player>() != null)
            {
                Debug.Log("Armored Orc hit the player with pierce attack.");
                // Additional logic for hitting the player with pierce attack can be added here
            }
        }
    }
}
