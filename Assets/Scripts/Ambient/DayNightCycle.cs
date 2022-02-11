using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle instance;

    [Range(0,24)] public float timeOfDay = 12;
    [SerializeField][Tooltip("Time in minutes")][Range(0.05f,60)] private float dayNightCylceDuration = 1;
    [SerializeField] private bool stopTime = false;
    [Space]
    [SerializeField] private AnimationCurve shadowPositionX;
    [SerializeField] private AnimationCurve shadowPositionY;
    [SerializeField] private AnimationCurve shadowOpacity;
    [Space]
    [SerializeField] private AnimationCurve lightBrightness;
    [Space]
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private Tilemap[] shadowsTilemaps;
    [SerializeField] private List<SpriteRenderer> shadowsSpriteRenderers;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    public void AddShadow(SpriteRenderer spriteRenderer)
    {
        shadowsSpriteRenderers.Add(spriteRenderer);
    }

    public void RemoveShadow(SpriteRenderer spriteRenderer)
    {
        if(shadowsSpriteRenderers.Contains(spriteRenderer)) shadowsSpriteRenderers.Remove(spriteRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTime && !GameManager.instance.GamePaused) SetTime();
        TimeAppearance();
    }

    private void SetTime()
    {
        timeOfDay += 24f / dayNightCylceDuration / 60 * Time.deltaTime;
        if (timeOfDay > 24) timeOfDay -= 24;
    }

    private void TimeAppearance()
    {
        foreach (var shadow in shadowsTilemaps)
        {
            shadow.transform.localPosition = new Vector2(shadowPositionX.Evaluate(timeOfDay), shadowPositionY.Evaluate(timeOfDay));
            shadow.color = new Color(0, 0, 0, shadowOpacity.Evaluate(timeOfDay));
        }
        foreach (var shadow in shadowsSpriteRenderers)
        {
            shadow.transform.localPosition = new Vector2(shadowPositionX.Evaluate(timeOfDay), shadowPositionY.Evaluate(timeOfDay));
            shadow.color = new Color(0, 0, 0, shadowOpacity.Evaluate(timeOfDay));
        }

        light.intensity = lightBrightness.Evaluate(timeOfDay);
    }
}
