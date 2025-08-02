using UnityEngine;

public class Enemy : Entity
{
    public Animator animator { get; private set; }

    public Transform playerPosition;

    #region State
    public EnemyStateMachine stateMachine { get; private set; }

    public EnemyIdleState idleState { get; private set; }
    public EnemyWanderState wanderState { get; private set; }
    public EnemyChaseState chaseState { get; private set; }
    public EnemyHurtState hurtState { get; private set; }
    public EnemyDeathState deathState { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        // Initialize the enemy's components
        animator = GetComponentInChildren<Animator>();
        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        wanderState = new EnemyWanderState(this, stateMachine, "Walk");
        chaseState = new EnemyChaseState(this, stateMachine, "Walk");
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
        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(chaseState);
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        // Set the enemy's velocity
        rb.linearVelocity = velocity;
    }

    public Vector2 GetPlayerPosition()
    {
        return playerPosition.position;
    }
}
