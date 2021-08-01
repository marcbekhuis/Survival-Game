using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasScaleHelper : Singleton<CanvasScaleHelper>
{
    private Canvas canvas;

    private void Awake()
    {
        base.Awake();
        canvas = GetComponent<Canvas>();
    }

    public float GetCanvasScale()
    {
        return canvas.scaleFactor;
    }
}
