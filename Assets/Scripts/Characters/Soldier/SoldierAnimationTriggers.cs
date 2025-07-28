using UnityEngine;
using Unity.Cinemachine;

public class SoldierAnimationTriggers : MonoBehaviour, IPlayerAnimationTriggers
{
    private Soldier soldier;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
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
            if (hit.GetComponent<Enemy>() != null) 
            {
                Debug.Log("Hit an enemy.");
            }
        }
    }

    private void ArrowAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(soldier.arrowAttackPoint.position, soldier.arrowAttackRange);
        Vector3 closestTargetPos = soldier.FindClosestTargetInArrowRange(colliders);
        GameObject arrow = Instantiate(soldier.arrowPrefab, soldier.attackPoint.position, Quaternion.identity);

        BasicArrowController arrowController = arrow.GetComponent<BasicArrowController>();
        arrowController.target = closestTargetPos;

    }


    private void ArrowChargeTrigger()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Arrow charge triggered.");
            soldier.animator.speed = 0;
            CameraManager.instance.StartCoroutine(CameraManager.instance.ZoomCamera(0.5f, 0.4f));
        }
    }

    private void SpecialAttackTrigger()
    {
        // Handle special attack logic here
        Debug.Log("Special attack triggered.");

        // Trigger camera shake effect
        float shakeVelocityX = Random.Range(-1f, 1f);
        float shakeVelocityY = Random.Range(-1f, 1f);
        impulseSource.DefaultVelocity = new Vector2(shakeVelocityX, shakeVelocityY);
        CameraShakeManager.instance.ShakeCamera(impulseSource);
    }
    
}
