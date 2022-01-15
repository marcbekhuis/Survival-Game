using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FireLight : MonoBehaviour
{
    [SerializeField] private AnimationCurve outerRadius;
    [SerializeField] private AnimationCurve brightness;
    private Light2D light;
    private float offset = 0;

    private void Start()
    {
        offset = Random.Range(0f, Mathf.Max(outerRadius.length, brightness.length));
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float animationTimeRadius = (Time.timeSinceLevelLoad + offset) - outerRadius.keys[outerRadius.length - 1].time * Mathf.Floor((Time.timeSinceLevelLoad + offset) / outerRadius.keys[outerRadius.length - 1].time);
        float animationTimeBrightness = (Time.timeSinceLevelLoad + offset) - brightness.keys[brightness.length - 1].time * Mathf.Floor((Time.timeSinceLevelLoad + offset) / brightness.keys[brightness.length - 1].time);

        light.pointLightOuterRadius = outerRadius.Evaluate(animationTimeRadius);
        light.intensity = brightness.Evaluate(animationTimeBrightness);
    }
}
