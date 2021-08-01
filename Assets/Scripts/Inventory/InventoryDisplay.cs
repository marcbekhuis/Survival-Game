using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] protected ItemContainer itemContainerPrefab;
    [SerializeField] protected Transform itemContainerParent;
    [SerializeField] protected bool movableContainers = true;

    protected List<ItemContainer> itemContainers = new List<ItemContainer>();

    private void Awake()
    {
        inventory.OnInventoryChanged += UpdateDisplay;
    }

    private void Start()
    {
        if (itemContainers.Count <= 0) UpdateDisplay();
    }

    public virtual void UpdateDisplay()
    {
        if (inventory.slots == null) return;
        Item[] items = new Item[inventory.slots.Length];
        int index = 0;
        for (int y = 0; y < inventory.size.y; y++)
        {
            for (int x = 0; x < inventory.size.x; x++)
            {
                items[index] = inventory.slots[x, y];
                index++;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (itemContainers.Count > i)
            {
                itemContainers[i].item = items[i];
            }
            else
            {
                int x = i;
                int y;
                for (y = 0; y < Mathf.FloorToInt((float)i / (float)inventory.slots.GetLength(0)); y++)
                {
                    x -= inventory.slots.GetLength(0);
                }
                ItemContainer itemContainer = Instantiate(itemContainerPrefab, itemContainerParent);
                itemContainers.Add(itemContainer);
                itemContainer.Setup(new Vector2Int(x, y), movableContainers, inventory);
                itemContainer.item = items[i];
            }
        }
    }
}
