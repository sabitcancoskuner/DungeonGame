using UnityEngine;

public class PlayerStats : EntityStats
{
    public Stat moveSpeed;
    public Stat attackSpeed;

    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);
    }
}
