using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : Usable
{
    public delegate void DurabilityCall(float value, float maxValue);
    public event DurabilityCall OnValueChanged = delegate { };
    public event DurabilityCall OnValueEmpty = delegate { };
    public event DurabilityCall OnValueFull = delegate { };

    [Header("Durability")]
    [Min(0)] public float maxDurability = 100;
    [Min(0)] public float useDurability = 1;

    protected float durabilityVar = 100;
    private bool empty = false;
    private bool full = false;

    public float durability
    {
        get => durabilityVar;
        set
        {
            durabilityVar = Mathf.Clamp(value, 0, maxDurability);
            OnValueChanged(durabilityVar, maxDurability);
            if (durabilityVar <= 0 && !empty)
            {
                OnValueEmpty(durabilityVar, maxDurability);
                empty = true;
            }
            else if (durabilityVar >= maxDurability && !full)
            {
                OnValueFull(durabilityVar, maxDurability);
                full = true;
            }
            else if (durabilityVar > 0 && empty)
            {
                //OnValueNoLongerEmpty(statValue, maxStatValue);
                empty = false;
            }
            else if (durabilityVar < maxDurability && full)
            {
                //OnValueNoLongerFull(statValue, maxStatValue);
                full = false;
            }
        }
    }

    public void DestroyItem(float value, float maxValue)
    {
        inventory.RemoveItem(this);
    }

    public Durability(Durability item, Inventory inventory, int amount = 1) : base(item, inventory, amount)
    {
        maxDurability = item.maxDurability;
        durabilityVar = maxDurability;
        useDurability = item.useDurability;
        OnValueEmpty += DestroyItem;
    }

    public Durability(string description, float weight, int maxStack, Sprite icon, Sprite sprite, Inventory inventory, float maxDurability, float useDurability, int amount = 1) : base(description, weight, maxStack, icon, sprite, inventory, amount)
    {
        this.maxDurability = maxDurability;
        durabilityVar = maxDurability;
        this.useDurability = useDurability;
        OnValueEmpty += DestroyItem;
    }

    public void UseDurability(Vector2 direction)
    {
        durability -= useDurability;
    }
}
