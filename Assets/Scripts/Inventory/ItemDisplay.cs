using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    private Item itemVar;

    private Text amountText;
    private Image iconImage;
    private Image durabilityBar;
    private Image Durabilityfill;
    private RectTransform rectTransform;
    private bool tooltipActivating = true;

    public Item item
    {
        get => itemVar;
        set
        {
            itemVar = value;
            if(itemVar) UpdateDislpay();
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(bool tooltipactivating)
    {
        amountText = transform.Find("Amount").GetComponent<Text>();
        iconImage = transform.Find("Icon").GetComponent<Image>();
        durabilityBar = transform.Find("Durability bar").GetComponent<Image>();
        Durabilityfill = durabilityBar.transform.Find("Durability fill").GetComponent<Image>();
        this.tooltipActivating = tooltipactivating;
        if(!tooltipActivating) TooltipHandler.instance.RemoveItemDisplay(this);
    }

    public void UpdateDislpay()
    {
        if (item.maxStack <= 1)
        {
            amountText.gameObject.SetActive(false);
        }
        else
        {
            amountText.text = item.amount.ToString();
            amountText.gameObject.SetActive(true);
        }
        iconImage.sprite = item.icon;
        if (item.IsType<Durability>())
        {
            Durability durability = item.GetType<Durability>();
            UpdateDurabilityBar(durability.durability, durability.maxDurability);
            durability.OnValueChanged += UpdateDurabilityBar;
        }
        else
        {
            durabilityBar.gameObject.SetActive(false);
        }
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
        if(tooltipActivating) TooltipHandler.instance.AddItemDisplay(this);
    }

    private void OnDisable()
    {
        if (tooltipActivating) TooltipHandler.instance.RemoveItemDisplay(this);
    }

    private void UpdateDurabilityBar(float durability, float maxDurability)
    {
        float procentage = durability / maxDurability;

        Durabilityfill.rectTransform.sizeDelta = new Vector2(durabilityBar.rectTransform.rect.width * procentage, durabilityBar.rectTransform.rect.height);
    }
}
