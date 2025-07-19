using UnityEngine;

public class PlayerBaseAttackState : PlayerState
{
    public PlayerBaseAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (baseAttackTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        baseAttackTriggerCalled = false;
    }
}
