using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering idle state can be added here
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating idle state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting idle state can be added here
    }
}
