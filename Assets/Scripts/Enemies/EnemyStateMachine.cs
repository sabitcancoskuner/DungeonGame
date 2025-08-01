using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState currentState;

    public void initialize(EnemyState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState nextState)
    {
        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
    }
}
