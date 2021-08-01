using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : ScriptableObject
{
    [Header("Resource")]
    public float maxHealth = 10;
    public RandomItem[] randomItems = new RandomItem[] { };
    [HideInInspector] public float health = 0;

    protected Vector2Int gridPosition;

    public Resource(Resource resource, Vector2Int gridPosition, float maxHealth)
    {
        randomItems = resource.randomItems;
        this.gridPosition = gridPosition;
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public virtual void SetHealth(float value, HarvestAttack harvestAttack)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        if (health <= 0)
        {
            RemoveFromTilemap();
            foreach (var randomItem in randomItems)
            {
                foreach (var inventory in harvestAttack.inventories)
                {
                    if (inventory.HasSpace() || inventory.HasItemSlotWithSpace(randomItem.item))
                    {
                        int amount = randomItem.GetAmount(harvestAttack.change);
                        inventory.AddItem(randomItem.item, amount);
                        break;
                    }
                }
            }
        }
    }

    public virtual void RemoveFromTilemap()
    {

    }
}
