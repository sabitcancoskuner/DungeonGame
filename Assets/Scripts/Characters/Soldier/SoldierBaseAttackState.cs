using UnityEngine;

public class SoldierBaseAttackState : PlayerBaseAttackState
{
    public SoldierBaseAttackState(Soldier _soldier, PlayerStateMachine _stateMachine, string _animName) : base(_soldier, _stateMachine, _animName)
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
