using UnityEngine;

public class ArmoredOrcPierceAttackState : EnemySecondaryAttackState
{
    public ArmoredOrcPierceAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering pierce attack state can be added here
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating pierce attack state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting pierce attack state can be added here
    }
}
