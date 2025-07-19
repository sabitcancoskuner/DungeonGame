using UnityEngine;

public class PlayerSecondaryAttackState : PlayerState
{
    public PlayerSecondaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (secondaryAttackTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        secondaryAttackTriggerCalled = false;
    }
}
