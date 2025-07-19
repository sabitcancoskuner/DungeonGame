using UnityEngine;

public class PlayerState
{
    public Player player { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    public string animBoolName { get; private set; }
    public bool baseAttackTriggerCalled = false;
    public bool secondaryAttackTriggerCalled = false;
    public bool specialAttackTriggerCalled = false;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        if (player.moveInput.x < 0 && player.facingDirection == 1 || player.moveInput.x > 0 && player.facingDirection == -1)
        {
            player.FlipSprite();
        }
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public void BaseAttackAnimationFinishTrigger()
    {
        baseAttackTriggerCalled = true;
    }

    public void SecondaryAttackAnimationFinishTrigger()
    {
        secondaryAttackTriggerCalled = true;
    }
    
    public void SpecialAttackAnimationFinishTrigger()
    {
        specialAttackTriggerCalled = true;
    }
}
