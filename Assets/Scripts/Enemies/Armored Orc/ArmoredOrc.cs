using UnityEngine;

public class ArmoredOrc : Enemy
{

    #region Armored Orc Specific States
    public ArmoredOrcBaseAttackState baseAttackState { get; private set; }
    public ArmoredOrcPierceAttackState pierceAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // Initialize the Armored Orc's specific states
        baseAttackState = new ArmoredOrcBaseAttackState(this, stateMachine, "Base Attack");
        pierceAttackState = new ArmoredOrcPierceAttackState(this, stateMachine, "Pierce Attack");
    }

    protected override void Start()
    {
        base.Start();
        // Additional initialization for ArmoredOrc can be added here
    }

    protected override void Update()
    {
        base.Update();
    }
}
