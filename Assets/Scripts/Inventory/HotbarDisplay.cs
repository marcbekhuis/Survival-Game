using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarDisplay : InventoryDisplay
{
    [SerializeField] private HotbarInventory hotbarInventory;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color SelectedColor = Color.red;

    public override void UpdateDisplay()
    {
        if (hotbarInventory.slots == null) return;
        Item[] items = new Item[hotbarInventory.slots.Length];
        int index = 0;
        for (int y = 0; y < hotbarInventory.size.y; y++)
        {
            for (int x = 0; x < hotbarInventory.size.x; x++)
            {
                items[index] = hotbarInventory.slots[x, y];
                index++;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (itemContainers.Count > i)
            {
                ItemContainer itemContainer = itemContainers[i];
                itemContainer.item = items[i];
                if (i == hotbarInventory.index) itemContainer.SetColor(SelectedColor);
                else itemContainer.SetColor(defaultColor);
            }
            else
            {
                ItemContainer itemContainer = Instantiate(itemContainerPrefab, itemContainerParent);
                itemContainers.Add(itemContainer);
                int x = i;
                int y;
                for (y = 0; y < Mathf.FloorToInt((float)i / (float)hotbarInventory.slots.GetLength(0)); y++)
                {
                    x -= hotbarInventory.slots.GetLength(0);
                }
                itemContainer.Setup(new Vector2Int(x,y), movableContainers, hotbarInventory);
                itemContainer.item = items[i];
                if (i == hotbarInventory.index) itemContainer.SetColor(SelectedColor);
                else itemContainer.SetColor(defaultColor);
            }
        }
    }
}
