using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputSystem inputSystemKeyboard = new InputSystemKeyboard();
    public InputSystem inputSystemController = new InputSystemController();

    public static InputManager instance;
    public InputSystem lastUsedInputSystem;

    public Dictionary<InputActions, List<KeyCode>> Keybinds
    {
        get
        {
            if (lastUsedInputSystem != null) return lastUsedInputSystem.keybinds;
            else return inputSystemKeyboard.keybinds;
        }
    }

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    public float GetAxis(InputAxis axis)
    {
        float controllerAxis = inputSystemController.GetAxis(axis);
        float keyboardAxis = inputSystemKeyboard.GetAxis(axis);

        if (Mathf.Abs(controllerAxis) > Mathf.Abs(keyboardAxis))
        {
            lastUsedInputSystem = inputSystemController;
            return controllerAxis;
        }
        else
        {
            lastUsedInputSystem = inputSystemKeyboard;
            return keyboardAxis;
        }
    }

    public bool GetKey(InputActions action)
    {
        bool output = false;
        if (inputSystemController.keybinds.ContainsKey(action))
            if (inputSystemController.GetKey(action)) {
                lastUsedInputSystem = inputSystemController;
                output = true;
            }
        if (inputSystemKeyboard.GetKey(action))
        {
            lastUsedInputSystem = inputSystemKeyboard;
            output = true;
        }
        return output;
    }

    public bool GetKeyDown(InputActions action)
    {
        bool output = false;
        if (inputSystemController.keybinds.ContainsKey(action)) 
            if (inputSystemController.GetKeyDown(action)) {
                lastUsedInputSystem = inputSystemController;
                output = true;
            }
        if (inputSystemKeyboard.GetKeyDown(action))
        {
            lastUsedInputSystem = inputSystemKeyboard;
            output = true;
        }
        return output;
    }

    public bool GetKeyUp(InputActions action)
    {
        bool output = false;
        if (inputSystemController.keybinds.ContainsKey(action))
            if (inputSystemController.GetKeyUp(action))
            {
                lastUsedInputSystem = inputSystemController;
                output = true;
            }
        if (inputSystemKeyboard.GetKeyUp(action))
        {
            lastUsedInputSystem = inputSystemKeyboard;
            output = true;
        }
        return output;
    }
}
