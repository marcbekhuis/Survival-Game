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

    private void Update()
    {
        if (GameManager.instance.panelOpen) return;

        for (int i = 0; i < inputActions.Length; i++)
        {
            if (InputManager.instance.GetKeyDown(inputActions[i]))
            {
                if (selectedItem)
                {
                    if (selectedItem.IsType<Usable>())
                    {
                        selectedItem.GetType<Usable>().RemoveUsage(gameObject, playerUseObjects);
                    }
                }
                int x = i;
                int y;
                for (y = 0; y < Mathf.FloorToInt((float)i / (float)slots.GetLength(0)); y++)
                {
                    x -= slots.GetLength(0);
                }
                index2d = new Vector2Int(x, y);
                index = i;
                OnInventoryChanged();
                if (selectedItem)
                {
                    if (selectedItem.IsType<Usable>())
                    {
                        selectedItem.GetType<Usable>().AddUsage(gameObject, playerUseObjects);
                    }
                }
                return;
            }
        }
    }
}
