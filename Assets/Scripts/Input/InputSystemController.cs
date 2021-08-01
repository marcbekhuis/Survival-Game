using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemController : InputSystem
{
    public new Dictionary<InputActions, List<KeyCode>> keybinds = new Dictionary<InputActions, List<KeyCode>>
    {
        { InputActions.Sprint, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.UseTool, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.DropItem, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.OpenConsole, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.OpenInventory, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot1, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot2, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot3, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot4, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot5, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot6, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot7, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot8, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.HotbarSlot9, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.MoveItem, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.Pause, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.Minimap, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.SkipTutorial, new List<KeyCode>{KeyCode.JoystickButton0}},
        { InputActions.TutorialNext, new List<KeyCode>{KeyCode.JoystickButton0}},
    };

    public override float GetAxis(InputAxis axis)
    {
        switch (axis)
        {
            case InputAxis.Horizontal:
                {
                    if (keybinds.ContainsKey(InputActions.MoveLeft) && keybinds.ContainsKey(InputActions.MoveRight))
                    {
                        float output = 0;
                        if (GetKey(InputActions.MoveLeft)) output -= 1;
                        if (GetKey(InputActions.MoveRight)) output += 1;
                        return output;
                    }
                    else return Input.GetAxis(axis.ToString());
                }
            case InputAxis.Vertical:
                {
                    if (keybinds.ContainsKey(InputActions.MoveBackward) && keybinds.ContainsKey(InputActions.MoveForward))
                    {
                        float output = 0;
                        if (GetKey(InputActions.MoveBackward)) output -= 1;
                        if (GetKey(InputActions.MoveForward)) output += 1;
                        return output;
                    }
                    else return Input.GetAxis(axis.ToString());
                }
            default:
                return 0;
        }
    }
}
