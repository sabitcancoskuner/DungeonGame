using UnityEngine;

public class EnemyState
{
    public Enemy enemy { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public string animBoolName { get; private set; }

    protected bool baseAttackTriggerCalled = false;
    protected bool secondaryAttackTriggerCalled = false;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        enemy.animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        // Default update logic for enemy states can be added here
    }

    public virtual void Exit()
    {
        enemy.animator.SetBool(animBoolName, false);
    }

    public virtual void BaseAttackAnimationFinishTrigger()
    {
        baseAttackTriggerCalled = true;
    }
    
    public virtual void SecondaryAttackAnimationFinishTrigger()
    {
        secondaryAttackTriggerCalled = true;
    }
}
