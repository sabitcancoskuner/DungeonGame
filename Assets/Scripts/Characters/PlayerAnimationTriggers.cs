using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationFinishTrigger()
    {
        player.stateMachine.currentState.AttackAnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackPoint.position, player.attackRange);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // change this to enemy
            {
                Debug.Log("Hit an enemy.");
            }
        }
    }
}
