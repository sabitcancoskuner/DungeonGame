using UnityEngine;

public class ArmoredOrcBaseAttackState : EnemyBaseAttackState
{
    public ArmoredOrc armoredOrc;

    public ArmoredOrcBaseAttackState(ArmoredOrc enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.armoredOrc = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (baseAttackTriggerCalled)
        {
            // Handle the end of the base attack animation
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        baseAttackTriggerCalled = false; // Reset the trigger for the next attack
    }
}
