using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem
{
    public Dictionary<InputActions, List<KeyCode>> keybinds = new Dictionary<InputActions, List<KeyCode>>
    {
        { InputActions.MoveForward, new List<KeyCode>{KeyCode.W, KeyCode.UpArrow} },
        { InputActions.MoveBackward, new List<KeyCode>{KeyCode.S, KeyCode.DownArrow}},
        { InputActions.MoveLeft, new List<KeyCode>{KeyCode.A, KeyCode.LeftArrow}},
        { InputActions.MoveRight, new List<KeyCode>{KeyCode.D, KeyCode.RightArrow}},
        { InputActions.Sprint, new List<KeyCode>{KeyCode.LeftShift, KeyCode.RightShift}},
        { InputActions.UseTool, new List<KeyCode>{KeyCode.Mouse0}},
        { InputActions.DropItem, new List<KeyCode>{KeyCode.Q}},
        { InputActions.OpenConsole, new List<KeyCode>{KeyCode.F1}},
        { InputActions.OpenInventory, new List<KeyCode>{KeyCode.Tab}},
        { InputActions.HotbarSlot1, new List<KeyCode>{KeyCode.Alpha1}},
        { InputActions.HotbarSlot2, new List<KeyCode>{KeyCode.Alpha2}},
        { InputActions.HotbarSlot3, new List<KeyCode>{KeyCode.Alpha3}},
        { InputActions.HotbarSlot4, new List<KeyCode>{KeyCode.Alpha4}},
        { InputActions.HotbarSlot5, new List<KeyCode>{KeyCode.Alpha5}},
        { InputActions.HotbarSlot6, new List<KeyCode>{KeyCode.Alpha6}},
        { InputActions.HotbarSlot7, new List<KeyCode>{KeyCode.Alpha7}},
        { InputActions.HotbarSlot8, new List<KeyCode>{KeyCode.Alpha8}},
        { InputActions.HotbarSlot9, new List<KeyCode>{KeyCode.Alpha9}},
        { InputActions.MoveItem, new List<KeyCode>{KeyCode.Mouse0}},
        { InputActions.Pause, new List<KeyCode>{KeyCode.Escape}},
        { InputActions.Minimap, new List<KeyCode>{KeyCode.M}},
        { InputActions.SkipTutorial, new List<KeyCode>{KeyCode.Backspace}},
        { InputActions.TutorialNext, new List<KeyCode>{KeyCode.Return}},
        { InputActions.ToggleFullscreen, new List<KeyCode>{KeyCode.F11}},
    };

    public virtual float GetAxis(InputAxis axis)
    {
        switch (axis)
        {
            case InputAxis.Horizontal:
                {
                    float output = 0;
                    if (GetKey(InputActions.MoveLeft)) output -= 1;
                    if (GetKey(InputActions.MoveRight)) output += 1;
                    return output;
                }
            case InputAxis.Vertical:
                {
                    float output = 0;
                    if (GetKey(InputActions.MoveBackward)) output -= 1;
                    if (GetKey(InputActions.MoveForward)) output += 1;
                    return output;
                }
            default:
                return 0;
        }
    }

    public virtual bool GetKey(InputActions action)
    {
        foreach (var keybind in keybinds[action])
        {
            if(Input.GetKey(keybind)) return true;
        }
        return false;
    }

    public virtual bool GetKeyDown(InputActions action)
    {
        foreach (var keybind in keybinds[action])
        {
            if (Input.GetKeyDown(keybind)) return true;
        }
        return false;
    }

    public virtual bool GetKeyUp(InputActions action)
    {
        foreach (var keybind in keybinds[action])
        {
            if (Input.GetKeyUp(keybind)) return true;
        }
        return false;
    }
}
