using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    public List<float> modifiers;

    public float GetValue()
    {
        float finalValue = baseValue;
        foreach (float modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void SetValue(float value)
    {
        this.baseValue = value;
    }

    public void SetBaseValue(float value)
    {
        this.baseValue = value;
    }

    public void AddModifier(float amount)
    {
        if (modifiers == null)
        {
            modifiers = new List<float>();
        }
        
        modifiers.Add(amount);
    }

    public void RemoveModifier(float amount)
    {
        if (modifiers != null)
        {
            modifiers.Remove(amount);
        }
    }
}
