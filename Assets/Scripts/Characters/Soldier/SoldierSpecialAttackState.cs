using UnityEngine;

public class SoldierSpecialAttackState : PlayerSpecialAttackState
{
    public SoldierSpecialAttackState(Soldier _soldier, PlayerStateMachine _stateMachine, string _animName) : base(_soldier, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

