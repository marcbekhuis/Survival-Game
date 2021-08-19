using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AspectPreserver : MonoBehaviour
{
    private Image image;
    Vector2 oldScreenSize;

    // Start is called before the first frame update
    void Update()
    {
        AspectRatio();
    }

    public void AspectRatio()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        if (screenSize == oldScreenSize) return;

        if (!image) image = GetComponent<Image>();

        Vector2 imageSize = image.sprite.rect.size;

        float scaleFactorX = screenSize.x / imageSize.x;
        float scaleFactorY = screenSize.y / imageSize.y;

        if (scaleFactorX > scaleFactorY)
        {
            image.rectTransform.sizeDelta = imageSize * scaleFactorX / CanvasScaleHelper.instance.GetCanvasScale();
        }
        else
        {
            image.rectTransform.sizeDelta = imageSize * scaleFactorY / CanvasScaleHelper.instance.GetCanvasScale();
        }
    }
}
