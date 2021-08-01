using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSystem : StatSystem
{
    private void Update()
    {
        if (GameManager.instance.GamePaused) return;

        StatValue -= Time.deltaTime;
    }
}
