using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New harvesting tool", menuName = "Items/Harvesting tool")]
public class HarvestingTool : Tool
{
    public HarvestingTool(Tool item, Inventory inventory, int amount = 1) : base(item, inventory, amount)
    {
    }

    public HarvestingTool(string description, float weight, int maxStack, Sprite icon, Sprite sprite, Inventory inventory, float maxDurability, float damage, float radius, float arc, int amount = 1) : base(description, weight, maxStack, icon, sprite, inventory, maxDurability, damage, radius, arc, amount)
    {
    }

    public override Item Create(Item item, Inventory inventory, int amount = 1)
    {
        return new HarvestingTool((HarvestingTool)item, inventory, amount);
    }

    public override void AddUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse += UseDurability;
        HarvestAttack harvestAttack = componentGameObject.GetComponent<HarvestAttack>();
        MeleeAttack meleeAttack = componentGameObject.GetComponent<MeleeAttack>();

        harvestAttack.SetSettings(damage, radius, arc);
        meleeAttack.SetSettings(damage, radius, arc);

        playerUseObjects.OnUse += harvestAttack.Attack;
        playerUseObjects.OnUse += meleeAttack.Attack;
    }

    public override void RemoveUsage(GameObject componentGameObject, PlayerUseObjects playerUseObjects)
    {
        playerUseObjects.OnUse -= UseDurability;
        playerUseObjects.OnUse -= componentGameObject.GetComponent<HarvestAttack>().Attack;
        playerUseObjects.OnUse -= componentGameObject.GetComponent<MeleeAttack>().Attack;
    }
}
