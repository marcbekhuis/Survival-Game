using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHandler : Singleton<TooltipHandler>
{
    public bool tooltipActivated = true;

    private List<ItemDisplay> itemDisplays = new List<ItemDisplay>();

    private void Update()
    {
        if (tooltipActivated)
        {
            foreach (var itemDisplay in itemDisplays)
            {
                if (itemDisplay.MouseOver(out Item item))
                {
                    Tooltip.instance.SetDisplayItem(item);
                    Tooltip.instance.SetPosition();
                    Tooltip.instance.ShowTooltip();
                    return;
                }
            }
        }
        Tooltip.instance.HideTooltip();
    }

    public void AddItemDisplay(ItemDisplay itemDisplay)
    {
        if(!itemDisplays.Contains(itemDisplay)) itemDisplays.Add(itemDisplay);
    }

    public void RemoveItemDisplay(ItemDisplay itemDisplay)
    {
        if (itemDisplays.Contains(itemDisplay)) itemDisplays.Remove(itemDisplay);
    }
}
