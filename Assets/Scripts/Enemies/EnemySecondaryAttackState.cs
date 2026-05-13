using UnityEngine;

public class EnemySecondaryAttackState : EnemyState
{
    public EnemySecondaryAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // logic for entering secondary attack state can be added here
        enemy.SetAlertAreaActive(false); // Disable alert area during attack
    }

    public override void Update()
    {
        base.Update();
        // Logic for updating secondary attack state can be added here
    }

    public override void Exit()
    {
        base.Exit();
        // logic for exiting secondary attack state can be added here
        enemy.SetAlertAreaActive(true); // Re-enable alert area when exiting attack
    }
}
