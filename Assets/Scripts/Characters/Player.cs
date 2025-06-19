using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] private InputActionReference move;
    public Vector2 moveInput { get; private set; }
    public Animator animator { get; private set; }

    #region States
    public PlayerStateMachine stateMachine;

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    // public PlayerState baseAttackState { get; private set; }
    // public PlayerState hurtState { get; private set; }
    // public PlayerState deathState { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();

        stateMachine.currentState.Update();

        if (moveInput.x < 0 && facingDirection == 1 || moveInput.x > 0 && facingDirection == -1)
        {
            FlipSprite();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
}
