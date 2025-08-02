using UnityEngine;

public class EnemyWanderState : EnemyState
{
    private Vector2 moveDirection;
    private float walkDuration;
    private float walkTimer;

    public EnemyWanderState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Set the animation state for walking
        walkDuration = Random.Range(0.5f, 2f);
        walkTimer = walkDuration;

        ChooseRandomDirection(); // choose a random direction to walk
        enemy.SetVelocity(moveDirection * enemy.moveSpeed); // set the velocity based on the chosen direction and speed
    }

    public override void Update()
    {
        base.Update();

        // Move the enemy in a random direction
        walkTimer -= Time.deltaTime;

        // Switch to idle state after walking for a certain duration
        if (walkTimer <= 0f)
        {
            stateMachine.ChangeState(((ArmoredOrc)enemy).idleState);
        }

        // Flip the sprite based on the movement direction
        if (moveDirection.x > 0 && enemy.facingDirection == -1)
        {
            enemy.FlipSprite();
        }
        else if (moveDirection.x < 0 && enemy.facingDirection == 1)
        {
            enemy.FlipSprite();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ChooseRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}
