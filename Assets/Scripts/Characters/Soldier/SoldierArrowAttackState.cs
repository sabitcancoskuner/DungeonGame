using UnityEngine;

public class SoldierArrowAttackState : PlayerSecondaryAttackState
{
    public Soldier soldier;

    public SoldierArrowAttackState(Soldier _soldier, PlayerStateMachine _stateMachine, string _animName) : base(_soldier, _stateMachine, _animName)
    {
        this.soldier = _soldier;
    }

    public override void Enter()
    {
        base.Enter();

        soldier.arrowAttackIndicator.SetActive(true);
    }

    public override void Update()
    {
        base.Update();

        if (soldier.animator.speed == 0) // if animation speed is 0 then it is charging an arrow
        {
            stateMachine.ChangeState(soldier.arrowChargingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        soldier.arrowAttackIndicator.SetActive(false);
    }
}
