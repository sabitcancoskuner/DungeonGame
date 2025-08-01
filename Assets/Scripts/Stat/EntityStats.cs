using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float currentHealth;

    public Stat maxHealth;
    public Stat defense;
    public Stat evasion;
    public Stat baseAttackDamage;


    private void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
    }
}
