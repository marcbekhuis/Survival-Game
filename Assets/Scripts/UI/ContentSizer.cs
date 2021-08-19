using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ContentSizer : MonoBehaviour
{
    [SerializeField] private bool sizeWidth = true;
    [SerializeField] private bool sizeHeight = true;

    RectTransform content;

    private void OnEnable()
    {
        SetContentSize();
    }

    public void SetContentSize()
    {
        if(!content) content = gameObject.GetComponent<RectTransform>();

        Vector2 maxPos = Vector2.zero;
        Vector2 minPos = Vector2.zero;

        foreach (RectTransform child in content)
        {
            if (!child.gameObject.activeSelf) continue;
            if(child.rect.xMax * CanvasScaleHelper.instance.GetCanvasScale() > maxPos.x) maxPos = new Vector2(child.rect.xMax * CanvasScaleHelper.instance.GetCanvasScale(), maxPos.y);
            if(child.rect.yMax * CanvasScaleHelper.instance.GetCanvasScale() > maxPos.y) maxPos = new Vector2(maxPos.x, child.rect.yMax * CanvasScaleHelper.instance.GetCanvasScale());
            if (child.rect.xMin * CanvasScaleHelper.instance.GetCanvasScale() < minPos.x) minPos = new Vector2(child.rect.xMin * CanvasScaleHelper.instance.GetCanvasScale(), minPos.y);
            if (child.rect.yMin * CanvasScaleHelper.instance.GetCanvasScale() < minPos.y) minPos = new Vector2(minPos.x, child.rect.yMin * CanvasScaleHelper.instance.GetCanvasScale());
            Debug.LogError(child.rect);
        }
        
        if(sizeHeight) content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Abs(maxPos.y) + Mathf.Abs(minPos.y));
        if(sizeWidth) content.sizeDelta = new Vector2(Mathf.Abs(maxPos.x) + Mathf.Abs(minPos.x), content.sizeDelta.y);
    }
}
