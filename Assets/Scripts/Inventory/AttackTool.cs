using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New attack tool", menuName = "Items/Attack tool")]
public class AttackTool : Tool
{
    public AttackTool(Tool item, Inventory inventory, int amount = 1) : base(item, inventory, amount)
    {
    }

    public AttackTool(string description, float weight, int maxStack, Sprite icon, Sprite sprite, Inventory inventory, float maxDurability, float useDurability, float damage, float radius, float arc, int amount = 1) : base(description, weight, maxStack, icon, sprite, inventory, maxDurability, useDurability, damage, radius, arc, amount)
    {
    }

    public override Item Create(Item item, Inventory inventory, int amount = 1)
    {
        return new AttackTool((AttackTool)item, inventory, amount);
    }

    public override void AddUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse += UseDurability;
        MeleeAttack meleeAttack = componentGameObject.GetComponent<MeleeAttack>();

        meleeAttack.SetSettings(damage, radius, arc);
        
        playerUseObjects.OnUse += meleeAttack.Attack;
    }

    public override void RemoveUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse -= UseDurability;
        playerUseObjects.OnUse -= componentGameObject.GetComponent<MeleeAttack>().Attack;
    }
}
