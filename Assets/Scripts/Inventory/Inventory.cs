using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void Changed();
    public Changed OnInventoryChanged = delegate { };

    [HideInInspector] public Item[,] slots;

    [Min(1)][SerializeField] private Vector2Int sizeVar = new Vector2Int(1,1);
    [SerializeField] private StartingItem[] startingItems;

    private void Start()
    {
        size = sizeVar;
        GetStartingItems();
    }

    public Vector2Int size
    {
        get => sizeVar;
        set
        {
            Vector2Int oldSize = sizeVar;
            sizeVar = value;
            Item[,] oldSlots = slots;
            bool emptySlots = false;
            if (slots == null) emptySlots = true;

            slots = new Item[sizeVar.x, sizeVar.y];
            List<Item> relocateItems = new List<Item>();

            if (emptySlots)
            {
                OnInventoryChanged();
                return;
            }

            for (int y = 0; y < oldSize.y; y++)
            {
                for (int x = 0; x < oldSize.x; x++)
                {
                    if (x > sizeVar.x || y > size.y)
                    {
                        relocateItems.Add(oldSlots[x, y]);
                        continue;
                    }
                    slots[x, y] = oldSlots[x, y];
                }
            }

            if (relocateItems.Count <= 0)
            {
                OnInventoryChanged();
                return;
            }

            for (int y = 0; y < oldSize.y; y++)
            {
                for (int x = 0; x < oldSize.x; x++)
                {
                    if (slots[x, y]) continue;
                    slots[x, y] = relocateItems[0];
                    relocateItems.RemoveAt(0);
                    if (relocateItems.Count <= 0)
                    {
                        OnInventoryChanged();
                        return;
                    }
                }
            }
        }
    }

    public void GetStartingItems()
    {
        foreach (var startingItem in startingItems)
        {
            AddItem(startingItem.item, startingItem.amount);
        }
    }

    public void AddItem(Item item, int amount)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (slots[x, y])
                {
                    if (slots[x, y].name.ToLower() != item.name.ToLower()) continue;
                    if (slots[x, y].amount >= slots[x, y].maxStack) continue;
                    int amountAvailable = slots[x, y].maxStack - slots[x, y].amount;
                    int amountToAdd = Mathf.Clamp(amountAvailable, 0, amount);
                    slots[x, y].amount += amountToAdd;
                    amount -= amountToAdd;
                    OnInventoryChanged();
                    if (amount <= 0) return;
                }
            }
        }

        if (amount <= 0) return;

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (slots[x, y]) continue;
                if (amount > item.maxStack)
                {
                    slots[x, y] = item.Create(item, this, item.maxStack);
                    OnInventoryChanged();
                    amount -= item.maxStack;
                }
                else
                {
                    slots[x, y] = item.Create(item, this, amount);
                    OnInventoryChanged();
                    return;
                }
            }
        }
    }

    public void AddItem(Item item)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (slots[x, y]) continue;
                slots[x, y] = item;
                item.inventory = this;
                OnInventoryChanged();
                return;
            }
        }
    }

    public void AddItem(Item item, Vector2Int index)
    {
        slots[index.x, index.y] = item;
        item.inventory = this;
        OnInventoryChanged();
    }

    public bool SlotHasItem(Vector2Int index)
    {
        return slots[index.x, index.y];
    }

    public int GetItemAmount(Item item)
    {
        int amount = 0;
        foreach (var slot in slots)
        {
            if (!slot) continue;
            if (slot.name.ToLower() == item.name.ToLower()) amount += slot.amount;
        }
        return amount;
    }

    public Item GetSlotItem(Vector2Int index)
    {
        return slots[index.x, index.y];
    }

    public bool HasSpace()
    {
        foreach (var slot in slots)
        {
            if (!slot) return true;
        }
        return false;
    }

    public bool HasItemSlotWithSpace(Item item)
    {
        foreach (var slot in slots)
        {
            if (slot.name.ToLower() == item.name.ToLower())
                if (slot.amount < slot.maxStack) return true;
        }
        return false;
    }

    public Item RemoveItem(Vector2Int index)
    {
        Item removingItem = slots[index.x, index.y];
        slots[index.x, index.y] = null;
        OnInventoryChanged();
        return removingItem;
    }
}
