using UnityEngine;

public class ArmoredOrcPierceAttackState : EnemySecondaryAttackState
{
    public ArmoredOrc armoredOrc;

    public ArmoredOrcPierceAttackState(ArmoredOrc enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
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
        
        if (secondaryAttackTriggerCalled)
        {
            // Handle the end of the pierce attack animation
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        secondaryAttackTriggerCalled = false; // Reset the trigger for the next attack
    }
}
