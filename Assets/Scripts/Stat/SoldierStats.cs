using UnityEngine;

public class SoldierStats : PlayerStats
{
    private Soldier soldier;

    // Class specific stats
    public Stat arrowDamage;
    public Stat arrowCount;

    private void Awake()
    {
        soldier = GetComponent<Soldier>();
    }
    
    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);

        if (health.GetValue() <= 0)
        {
            // Handle soldier death logic here
            StartCoroutine(soldier.Die());
            Debug.Log("Soldier has died.");
            // You can trigger animations, effects, or game over logic here
        }
    }
}
