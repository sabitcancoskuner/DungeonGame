using UnityEngine;

public class ArmoredOrcBaseAttackState : EnemyBaseAttackState
{
    public ArmoredOrcBaseAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering base attack state can be added here
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating base attack state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting base attack state can be added here
    }
}
