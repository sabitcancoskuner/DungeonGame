using UnityEngine;

public class EnemySpecialAttackState : EnemyState
{
    public EnemySpecialAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering special attack state can be added here
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating special attack state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting special attack state can be added here
    }
}
