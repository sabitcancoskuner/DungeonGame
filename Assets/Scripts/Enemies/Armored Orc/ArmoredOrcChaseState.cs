using UnityEngine;

public class ArmoredOrcChaseState : EnemyChaseState
{
    public ArmoredOrc armoredOrc;

    public ArmoredOrcChaseState(ArmoredOrc enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.armoredOrc = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        // Additional logic specific to Armored Orc can be added here
    }

    public override void Update()
    {
        base.Update();
        // Additional logic specific to Armored Orc can be added here

        if (distanceToPlayer < .8f)
        {
            armoredOrc.SetZeroVelocity();
            // If player is within a certain range, switch to attack state
            float randomFloat = Random.Range(0f, 1f);
            if (randomFloat < 0.5f)
            {
                stateMachine.ChangeState(armoredOrc.baseAttackState);
            }
            else
            {
                stateMachine.ChangeState(armoredOrc.pierceAttackState);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        // Additional logic specific to Armored Orc can be added here
    }
}
