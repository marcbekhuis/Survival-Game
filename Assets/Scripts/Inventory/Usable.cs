using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    public Usable(Item item, Inventory inventory, int amount = 1) : base(item, inventory, amount)
    {
    }

    public Usable(string description, float weight, int maxStack, Sprite icon, Sprite sprite, Inventory inventory, int amount = 1) : base(description, weight, maxStack, icon, sprite, inventory, amount)
    {
    }

    public virtual void AddUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse += componentGameObject.GetComponent<AttackSystem>().Attack;
    }

    public virtual void RemoveUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse -= componentGameObject.GetComponent<AttackSystem>().Attack;
    }
}
