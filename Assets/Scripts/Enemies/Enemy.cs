using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Animator animator { get; private set; }

    public Transform playerPosition;

    public PolygonCollider2D alertAreaCollider;

    // Attack
    private bool canAttack = true;
    public Transform baseAttackPoint;
    public float baseAttackRange = 1f;

    public bool isDead;

    #region State
    public EnemyStateMachine stateMachine { get; private set; }

    public EnemyIdleState idleState { get; private set; }
    public EnemyWanderState wanderState { get; private set; }
    public EnemyHurtState hurtState { get; private set; }
    public EnemyDeathState deathState { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        // Initialize the enemy's components
        animator = GetComponentInChildren<Animator>();
        alertAreaCollider = GetComponentInChildren<PolygonCollider2D>();

        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        wanderState = new EnemyWanderState(this, stateMachine, "Walk");
        hurtState = new EnemyHurtState(this, stateMachine, "Hurt");
        deathState = new EnemyDeathState(this, stateMachine, "Death");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.initialize(idleState);
    }

    protected virtual void Update()
    {
        if (isDead) return;
        stateMachine.currentState.Update();
    }

    public void SetVelocity(Vector2 velocity)
    {
        // Set the enemy's velocity
        rb.linearVelocity = velocity;
    }

    public Vector2 GetPlayerPosition()
    {
        if (playerPosition == null) return transform.position;
        return playerPosition.position;
    }

    protected virtual void OnDrawGizmos()
    {
        if (baseAttackPoint != null)
        {
            // Draw Gizmos for Armored Orc Base Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(baseAttackPoint.position, baseAttackRange);
        }
    }

    public void SetAttackStatus(bool status)
    {
        canAttack = status;
    }
    
    public void SetAlertAreaActive(bool isActive)
    {
        alertAreaCollider.enabled = isActive;
    }

    public virtual IEnumerator Die()
    {
        stateMachine.ChangeState(deathState);
        yield return null; // wait for end of frame before reading
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }

}
