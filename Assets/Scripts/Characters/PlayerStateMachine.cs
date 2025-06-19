using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _nextState)
    {
        currentState.Exit();
        currentState = _nextState;
        currentState.Enter();
    }
}
