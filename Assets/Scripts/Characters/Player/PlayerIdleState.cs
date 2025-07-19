using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (player.moveInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.walkState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
