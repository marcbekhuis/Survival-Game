using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : Durability
{
    [Header("Tool")]
    [Min(0)] public float damage = 1;
    [Min(0)] public float radius = 1;
    [Min(0)] public float arc = 90;

    public Tool(Tool item, Inventory inventory, int amount = 1) : base(item, inventory, amount)
    {
        damage = item.damage;
        radius = item.radius;
        arc = item.arc;
    }

    public Tool(string description, float weight, int maxStack, Sprite icon, Sprite sprite, Inventory inventory, float maxDurability, float useDurability, float damage, float radius, float arc, int amount = 1) : base(description, weight, maxStack, icon, sprite, inventory, maxDurability, useDurability, amount)
    {
        this.damage = damage;
        this.radius = radius;
        this.arc = arc;
    }
}
