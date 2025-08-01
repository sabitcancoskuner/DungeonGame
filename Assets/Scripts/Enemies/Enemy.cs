using UnityEngine;

public class Enemy : Entity
{
    public Animator animator;

    protected override void Start()
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();
    }
}
