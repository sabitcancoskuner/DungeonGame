using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.rb.linearVelocity = player.moveInput * player.moveSpeed;

        if (player.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
