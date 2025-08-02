using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private float knockbackDuration;

    public PlayerHurtState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetAttackStatus(false);
        knockbackDuration = player.knockbackDuration;
        player.HitKnockback(player.knockbackSpeed);
    }

    public override void Update()
    {
        base.Update();

        knockbackDuration -= Time.deltaTime;

        if (knockbackDuration <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAttackStatus(true);
    }
}
