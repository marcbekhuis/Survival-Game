using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : Singleton<ItemMovement>
{
    [HideInInspector] public List<ItemContainer> itemContainers = new List<ItemContainer>();

    [SerializeField] private ItemDisplay itemDisplayPrefab;
    [SerializeField] private Canvas canvas;
    private bool movementActiveVar = false;
    private ItemDisplay itemDisplay;
    private Item movingItemVar;
    private RectTransform itemDisplayRectTransform;

    public bool movementActive
    {
        get => movementActiveVar;
        set
        {
            movementActiveVar = value;
            if (movementActiveVar) return;
            if (!movingItem) return;
            foreach (var itemContainer in itemContainers)
            {
                if (itemContainer.item) continue;
                itemContainer.inventory.AddItem(movingItem, itemContainer.index);
                movingItem = null;
                return;
            }
        }
    }

    private Item movingItem
    {
        get => movingItemVar;
        set
        {
            movingItemVar = value;
            if (!movingItemVar)
            {
                TooltipHandler.instance.tooltipActivated = true;
                if (itemDisplay)
                {
                    Destroy(itemDisplay.gameObject);
                    itemDisplay = null;
                }
            }
            else if (itemDisplay)
            {
                itemDisplay.item = movingItemVar;
                TooltipHandler.instance.tooltipActivated = false;
            }
            else
            {
                itemDisplay = Instantiate(itemDisplayPrefab, canvas.transform);
                itemDisplayRectTransform = itemDisplay.GetComponent<RectTransform>();
                SetItemDisplayPosition();

                itemDisplay.Setup(false);
                itemDisplay.item = movingItemVar;
                TooltipHandler.instance.tooltipActivated = false;
            }
        }
    }

    private void Update()
    {
        if (!movementActive) return;
        if (itemDisplay) SetItemDisplayPosition();

        if (InputManager.instance.GetKeyDown(InputActions.MoveItem))
        {
            foreach (var itemContainer in itemContainers)
            {
                if(itemContainer.MouseOver(out Item item))
                {
                    Item newItem = null;
                    if (item) newItem = itemContainer.inventory.RemoveItem(itemContainer.index);
                    if (movingItem) itemContainer.inventory.AddItem(movingItem, itemContainer.index);
                    movingItem = newItem;
                    return;
                }
            }
        }
    }

    public void AddItemContainer(ItemContainer itemContainer)
    {
        if (!itemContainers.Contains(itemContainer)) itemContainers.Add(itemContainer);
    }

    public void RemoveItemContainer(ItemContainer itemContainer)
    {
        if (itemContainers.Contains(itemContainer)) itemContainers.Remove(itemContainer);
    }

    public void SetItemDisplayPosition()
    {
        MoveItemDisplayAnchorPoint(Input.mousePosition);
        itemDisplayRectTransform.position = Input.mousePosition;
    }

    private void MoveItemDisplayAnchorPoint(Vector2 mousePosition)
    {
        Vector2 tooltipSize = itemDisplayRectTransform.rect.size * CanvasScaleHelper.instance.GetCanvasScale();
        Vector2 screenSize = Camera.main.pixelRect.size;
        Vector2 pivot = new Vector2(0, 1);

        if (mousePosition.y - tooltipSize.y < 0) pivot = new Vector2(pivot.x, 0);
        if (mousePosition.x + tooltipSize.x > screenSize.x) pivot = new Vector2(1, pivot.y);

        itemDisplayRectTransform.pivot = pivot;
    }
}
