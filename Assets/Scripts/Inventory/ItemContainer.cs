using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [HideInInspector] public ItemDisplay itemDisplay;
    [HideInInspector] public Vector2Int index;
    [HideInInspector] public Inventory inventory;

    [SerializeField] private ItemDisplay itemDisplayPrefab;
    private Item itemVar;
    private Image backgroundImage;
    private RectTransform rectTransform;
    private bool movableContainer = true;

    public Item item
    {
        get => itemVar;
        set
        {
            if (!value)
            {
                itemVar = value;
                if (!itemDisplay) return;
                Destroy(itemDisplay.gameObject);
                itemDisplay = null;
            }
            else if (itemVar)
            {
                itemVar = value;
                itemDisplay.item = item;
            }
            else
            {
                itemVar = value;
                itemDisplay = Instantiate(itemDisplayPrefab, transform);
                itemDisplay.Setup(true);
                itemDisplay.item = item;
            }
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(Vector2Int index, bool movableContainer, Inventory inventory)
    {
        backgroundImage = GetComponent<Image>();
        this.index = index;
        this.movableContainer = movableContainer;
        this.inventory = inventory;
        if(!movableContainer) ItemMovement.instance.RemoveItemContainer(this);
    }

    public void SetColor(Color color)
    {
        backgroundImage.color = color;
    }

    public bool MouseOver(out Item item)
    {
        Vector2 halfSize = rectTransform.rect.size * 0.5f * CanvasScaleHelper.instance.GetCanvasScale();
        float highPosition = rectTransform.position.y + halfSize.y;
        float lowPosition = rectTransform.position.y - halfSize.y;
        float leftPosition = rectTransform.position.x - halfSize.x;
        float rightPosition = rectTransform.position.x + halfSize.x;
        if (Input.mousePosition.x < rightPosition && Input.mousePosition.x > leftPosition && Input.mousePosition.y < highPosition && Input.mousePosition.y > lowPosition)
        {
            item = itemVar;
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }

    private void OnEnable()
    {
        if(movableContainer) ItemMovement.instance.AddItemContainer(this);
    }

    private void OnDisable()
    {
        if (movableContainer) ItemMovement.instance.RemoveItemContainer(this);
    }
}
