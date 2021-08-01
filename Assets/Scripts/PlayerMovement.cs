using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused || GameManager.instance.PlayerMovementDisabled)
        {
            Move(Vector2.zero, false);
            return;
        }

        Vector2 input = new Vector2(InputManager.instance.GetAxis(InputAxis.Horizontal), InputManager.instance.GetAxis(InputAxis.Vertical));
        Move(input.normalized, InputManager.instance.GetKey(InputActions.Sprint));
    }
}
