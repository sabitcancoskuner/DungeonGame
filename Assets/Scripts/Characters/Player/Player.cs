using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Space]
    [SerializeField] protected InputActionReference move;
    [SerializeField] protected InputActionReference baseAttack;
    [SerializeField] protected InputActionReference secondaryAttack;
    [SerializeField] protected InputActionReference specialAttack;

    public Vector2 moveInput { get; private set; }
    public Animator animator { get; private set; }

    public Transform attackPoint;
    public float attackRange;

    #region States
    public PlayerStateMachine stateMachine;

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
        hurtState = new PlayerHurtState(this, stateMachine, "Hurt");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected virtual void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();

        stateMachine.currentState.Update();
    }

    private void OnDrawGizmos()
    {
        // Attack range visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
}
