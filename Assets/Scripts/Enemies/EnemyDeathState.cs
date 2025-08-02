using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating death state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting death state can be added here
    }
}
