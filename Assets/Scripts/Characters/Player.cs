using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference baseAttack;
    [SerializeField] private InputActionReference specialAttack;
    public Vector2 moveInput { get; private set; }
    public Animator animator { get; private set; }

    public Transform attackPoint;
    public float attackRange;

    #region States
    public PlayerStateMachine stateMachine;

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerBaseAttackState baseAttackState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
        baseAttackState = new PlayerBaseAttackState(this, stateMachine, "BaseAttack");
        hurtState = new PlayerHurtState(this, stateMachine, "Hurt");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            stateMachine.ChangeState(hurtState);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(deathState);
        }
    }

    private void OnEnable()
    {
        baseAttack.action.started += Attack;
        specialAttack.action.started += SpecialAttack;
    }

    private void OnDisable()
    {
        baseAttack.action.started -= Attack;
        specialAttack.action.started -= SpecialAttack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(baseAttackState);
    }

    private void SpecialAttack(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    
}
