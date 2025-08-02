using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float idleTimer;
    private float idleDuration;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering idle state can be added here
        enemy.SetZeroVelocity();

        // Random idle duration
        idleDuration = Random.Range(1f, 4f);
        idleTimer = idleDuration;
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating idle state can be added here

        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            // Transition to walk state after idle duration
            enemy.stateMachine.ChangeState(enemy.wanderState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting idle state can be added here
    }
}
