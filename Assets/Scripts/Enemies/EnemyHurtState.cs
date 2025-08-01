using UnityEngine;

public class EnemyHurtState : EnemyState
{
    public EnemyHurtState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering hurt state can be added here
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating hurt state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting hurt state can be added here
    }
}
