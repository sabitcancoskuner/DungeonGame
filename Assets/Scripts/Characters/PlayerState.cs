using UnityEngine;

public class PlayerState
{
    public Player player { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    public string animBoolName { get; private set; }

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
    
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }
}
