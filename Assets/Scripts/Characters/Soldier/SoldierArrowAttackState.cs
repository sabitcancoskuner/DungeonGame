using UnityEngine;

public class SoldierArrowAttackState : PlayerSecondaryAttackState
{
    public SoldierArrowAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (!Input.GetKey(KeyCode.Mouse1) && player.animator.speed == 0)
        {
            player.animator.speed = 1; // continue secondary attack animation when right click is released
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
