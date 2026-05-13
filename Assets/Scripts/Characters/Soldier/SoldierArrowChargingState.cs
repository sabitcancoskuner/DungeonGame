using UnityEngine;

public class SoldierArrowChargingState : SoldierArrowAttackState
{
    public SoldierArrowChargingState(Soldier _soldier, PlayerStateMachine _stateMachine, string _animName) : base(_soldier, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    { 
        base.Update();
        
        if (!player.secondaryAttack.action.IsPressed() && base.player.animator.speed == 0)
        {
            soldier.animator.speed = 1; // continue secondary attack animation when right click is released
            CameraManager.instance.StartCoroutine(CameraManager.instance.UnzoomCamera(0.5f, 0.3f));
            stateMachine.ChangeState(soldier.arrowAttackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
