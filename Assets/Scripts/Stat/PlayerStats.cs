using UnityEngine;

public class PlayerStats : EntityStats
{
    public Stat moveSpeed;
    public Stat attackSpeed;

    public Player player;

    protected virtual void Awake() {
        player = GetComponent<Player>();
    }

    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);
    }
}
