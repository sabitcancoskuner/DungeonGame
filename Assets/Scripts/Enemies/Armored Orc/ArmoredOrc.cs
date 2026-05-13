using UnityEngine;

public class ArmoredOrc : Enemy
{
    [Header("Armored Orc Specific Settings")]
    [SerializeField] public Transform pierceAttackPoint;
    public float pierceAttackRange = 2f;

    #region Armored Orc Specific States
    public ArmoredOrcChaseState chaseState { get; private set; }
    public ArmoredOrcBaseAttackState baseAttackState { get; private set; }
    public ArmoredOrcPierceAttackState pierceAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // Initialize the Armored Orc's specific states
        chaseState = new ArmoredOrcChaseState(this, stateMachine, "Walk");
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

    override protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (pierceAttackPoint != null)
        {
            // Draw Gizmos for Armored Orc Pierce Attack Range
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pierceAttackPoint.position, pierceAttackPoint.position + Vector3.right * pierceAttackRange * facingDirection); // Example line for visualization
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        // Check if the player has entered the alert area
        if (other.GetComponent<Player>() != null)
        {
            stateMachine.ChangeState(chaseState);
            playerPosition = other.transform; // Update player position for chasing
        }
    }
}
