using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private float knockbackDuration;

    public EnemyHurtState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        knockbackDuration = enemy.knockbackDuration;
        enemy.HitKnockback(enemy.knockbackSpeed);
    }

    public override void Update()
    {
        base.Update();

        knockbackDuration -= Time.deltaTime;

        if (knockbackDuration <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
