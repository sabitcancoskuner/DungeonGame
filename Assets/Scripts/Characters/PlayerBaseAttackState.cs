using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseAttackState : PlayerState
{
    public PlayerBaseAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (attackTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        attackTriggerCalled = false;
    }
}
