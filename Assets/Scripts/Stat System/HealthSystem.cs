using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : StatSystem
{
    [SerializeField] private FoodSystem foodSystem;
    [SerializeField] private ThurstSystem thurstSystem;

    private bool starving = false;
    private bool dehydrated = false;

    private void Awake()
    {
        if (foodSystem)
        {
            foodSystem.OnValueEmpty += EnableStarving;
            foodSystem.OnValueNoLongerEmpty += DisableStarving;
        }
        if (thurstSystem)
        {
            thurstSystem.OnValueEmpty += EnableDehydration;
            thurstSystem.OnValueNoLongerEmpty += DisableDehydration;
        }
    }

    private void Update()
    {
        if (GameManager.instance.GamePaused) return;

        if (starving)
        {
            StatValue -= Time.deltaTime;
        }
        if (dehydrated)
        {
            StatValue -= Time.deltaTime;
        }
    }

    private void EnableStarving(float value, float maxvalue)
    {
        starving = true;
    }

    private void DisableStarving(float value, float maxvalue)
    {
        starving = false;
    }

    private void EnableDehydration(float value, float maxvalue)
    {
        dehydrated = true;
    }

    private void DisableDehydration(float value, float maxvalue)
    {
        dehydrated = false;
    }
}
