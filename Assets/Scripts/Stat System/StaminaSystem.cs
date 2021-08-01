using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : StatSystem
{
    [SerializeField] private Movement movement;

    private void Awake()
    {
        movement.OnIdle += AddIdleStamina;
        movement.OnWalk += AddWalkStamina;
        movement.OnRunning += ConsumeRunStamina;

        OnValueEmpty += EnableExhausted;
        OnValueNoLongerEmpty += DisableExhausted;
    }

    private void ConsumeRunStamina(Vector2 velocity)
    {
        StatValue -= Time.deltaTime;
    }

    private void AddWalkStamina(Vector2 velocity)
    {
        StatValue += Time.deltaTime;
    }

    private void AddIdleStamina(Vector2 velocity)
    {
        StatValue += Time.deltaTime * 2f;
    }

    private void EnableExhausted(float value, float maxValue)
    {
        movement.exhausted = true;
    }

    private void DisableExhausted(float value, float maxValue)
    {
        movement.exhausted = false;
    }
}
