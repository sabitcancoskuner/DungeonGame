using UnityEngine;

public class SoldierStats : PlayerStats
{
    private Soldier soldier;

    // Class specific stats
    public Stat arrowDamage;
    public Stat arrowCount;

    // Special attack stats
    public Stat specialAttackDamage;
    public Stat specialAttackRange;

    [SerializeField] private SoldierStatSO soldierStatSO;

    private void Awake()
    {
        soldier = GetComponent<Soldier>();

        // Set stats base value from the StatSO
        this.maxHealth.SetBaseValue(soldierStatSO.health);
        this.defense.SetBaseValue(soldierStatSO.defense);
        this.evasion.SetBaseValue(soldierStatSO.evasion);
        this.moveSpeed.SetBaseValue(soldierStatSO.moveSpeed);
        this.attackSpeed.SetBaseValue(soldierStatSO.attackSpeed);
        this.baseAttackDamage.SetBaseValue(soldierStatSO.baseAttackDamage);
        this.arrowDamage.SetBaseValue(soldierStatSO.arrowDamage);
        this.arrowCount.SetBaseValue(soldierStatSO.arrowCount);
        this.specialAttackDamage.SetBaseValue(soldierStatSO.specialAttackDamage);
        this.specialAttackRange.SetBaseValue(soldierStatSO.specialAttackRange);
    }
    
    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);

        if (currentHealth <= 0)
        {
            // Handle soldier death logic here
            StartCoroutine(soldier.Die());
            Debug.Log("Soldier has died.");
            // You can trigger animations, effects, or game over logic here
        }
    }
}
