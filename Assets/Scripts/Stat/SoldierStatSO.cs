using UnityEngine;

[CreateAssetMenu(fileName = "SoldierStatSO", menuName = "StatSO/SoldierStatSO", order = 1)]
public class SoldierStatSO : ScriptableObject
{
    public float health;
    public float defense;
    public float evasion;
    public float moveSpeed;
    public float attackSpeed;
    public float baseAttackDamage;
    public float arrowDamage;
    public float arrowCount;
    public float specialAttackDamage;
    public float specialAttackRange;

}
