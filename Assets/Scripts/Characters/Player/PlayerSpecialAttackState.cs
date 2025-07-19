using UnityEngine;

public class PlayerSpecialAttackState : PlayerState
{
    public PlayerSpecialAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (specialAttackTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        specialAttackTriggerCalled = false;
    }
}
