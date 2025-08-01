using UnityEngine;

public class EnemyWalkState : EnemyState
{
    public EnemyWalkState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering walk state can be added here
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating walk state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting walk state can be added here
    }
}
