using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarInventory : Inventory
{
    [SerializeField] private PlayerUseObjects playerUseObjects;
    [SerializeField] private InputActions[] inputActions;

    public Item selectedItem
    {
        get => slots[index2d.x, index2d.y];
    }

    public Vector2Int index2d = Vector2Int.zero;
    public int index = 0;

    private void Start()
    {
        base.Start();
        SelectItem();
    }

    private void Update()
    {
        if (GameManager.instance.panelOpen) return;
        
        for (int i = 0; i < inputActions.Length; i++)
        {
            if (InputManager.instance.GetKeyDown(inputActions[i]))
            {
                DeselectItem();
                SetIndex(i);
                SelectItem();

                OnInventoryChanged();
                return;
            }
        }
    }

    public void DeselectItem() 
    {
        if (selectedItem)
        {
            if (selectedItem.IsType<Usable>())
            {
                selectedItem.GetType<Usable>().RemoveUsage(gameObject, playerUseObjects);
            }
        }
    }

    public void SetIndex(int i)
    {
        int x = i;
        int y;
        for (y = 0; y < Mathf.FloorToInt((float)i / (float)slots.GetLength(0)); y++)
        {
            x -= slots.GetLength(0);
        }
        index2d = new Vector2Int(x, y);
        index = i;
    }

    public void SelectItem()
    {
        if (selectedItem)
        {
            if (selectedItem.IsType<Usable>())
            {
                selectedItem.GetType<Usable>().AddUsage(gameObject, playerUseObjects);
            }
        }
    }

    public override Item RemoveItem(Item item)
    {
        if (selectedItem == item) DeselectItem();
        Item removedItem = base.RemoveItem(item);
        return removedItem;
    }

    public override Item RemoveItem(Vector2Int index)
    {
        if (selectedItem == GetSlotItem(index)) DeselectItem();
        Item removedItem = base.RemoveItem(index);
        return removedItem;
    }

    public override Item AddItem(Item item)
    {
        Item addedItem = base.AddItem(item);
        SelectItem();
        return addedItem;
    }

    public override Item AddItem(Item item, int amount)
    {
        Item addedItem = base.AddItem(item, amount);
        SelectItem();
        return addedItem;
    }
    
    public override Item AddItem(Item item, Vector2Int index)
    {
        Item addedItem = base.AddItem(item, index);
        SelectItem();
        return addedItem;
    }
}
