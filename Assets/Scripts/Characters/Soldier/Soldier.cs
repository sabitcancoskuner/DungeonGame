using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soldier : Player
{

    #region Character Specific States
    public PlayerBaseAttackState baseAttackState { get; private set; }
    public PlayerSecondaryAttackState secondaryAttackState { get; private set; }
    public PlayerSpecialAttackState specialAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        baseAttackState = new SoldierBaseAttackState(this, stateMachine, "BaseAttack");
        secondaryAttackState = new SoldierArrowAttackState(this, stateMachine, "SecondaryAttack");
        specialAttackState = new SoldierSpecialAttackState(this, stateMachine, "SpecialAttack");
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(baseAttackState);
    }

    private void SecondaryAttack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(secondaryAttackState);
    }

    private void SpecialAttack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(specialAttackState);
    }
    
    private void OnEnable()
    {
        baseAttack.action.started += Attack;
        secondaryAttack.action.started += SecondaryAttack;
        specialAttack.action.started += SpecialAttack;
    }

    private void OnDisable()
    {
        baseAttack.action.started -= Attack;
        secondaryAttack.action.started -= SecondaryAttack;
        specialAttack.action.started -= SpecialAttack;
    }

}
