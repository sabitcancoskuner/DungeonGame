using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : Entity
{
    [SerializeField] private InputActionReference move;
    protected Vector2 moveInput;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
}
