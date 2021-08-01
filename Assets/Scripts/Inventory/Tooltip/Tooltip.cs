using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip>
{
    [SerializeField] private Text informationText;
    [SerializeField] private Image iconImage;

    private Item displayedItem;
    private RectTransform rectTransform;

    private void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        HideTooltip();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }

    public void SetDisplayItem(Item item)
    {
        displayedItem = item;
        informationText.text = displayedItem.GetTooltipText();
        iconImage.sprite = displayedItem.icon;
        ShowTooltip();
    }

    public void SetPosition()
    {
        MoveAnchorPoint(Input.mousePosition);
        rectTransform.position = Input.mousePosition;
    }

    private void MoveAnchorPoint(Vector2 mousePosition)
    {
        Vector2 tooltipSize = rectTransform.rect.size * CanvasScaleHelper.instance.GetCanvasScale();
        Vector2 screenSize = Camera.main.pixelRect.size;
        Vector2 pivot = new Vector2(0, 1);

        if (mousePosition.y - tooltipSize.y < 0) pivot = new Vector2(pivot.x, 0);
        if (mousePosition.x + tooltipSize.x > screenSize.x) pivot = new Vector2(1, pivot.y);

        rectTransform.pivot = pivot;
    }
}
