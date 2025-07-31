using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat health;
    public Stat defense;
    public Stat evasion;
    public Stat baseAttackDamage;

    public virtual void DecreaseHealth(float amount)
    {
        health.SetValue(health.GetValue() - amount);
    }
}
