using System.Collections;
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

    public bool canAttack = true;
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
        if (isDead) return;
        
        moveInput = move.action.ReadValue<Vector2>();

        stateMachine.currentState.Update();
    }

    protected virtual void OnDrawGizmos()
    {
        // Attack range visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void SetAttackStatus(bool status)
    {
        canAttack = status;
    }

    public IEnumerator Die()
    {
        stateMachine.ChangeState(deathState);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Time.timeScale = 0f; // Pause the game
        // Additional logic for player death can be added here
    }
}
