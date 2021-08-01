using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeycodeVisualizor
{
    private static Dictionary<KeyCode, string> VisualKeycodes = new Dictionary<KeyCode, string> 
    { 
        {KeyCode.Alpha1, "1"},
        {KeyCode.Alpha2, "2"},
        {KeyCode.Alpha3, "3"},
        {KeyCode.Alpha4, "4"},
        {KeyCode.Alpha5, "5"},
        {KeyCode.Alpha6, "6"},
        {KeyCode.Alpha7, "7"},
        {KeyCode.Alpha8, "8"},
        {KeyCode.Alpha9, "9"},
        {KeyCode.Mouse0, "Left mouse"},
        {KeyCode.Mouse1, "Right mouse"},
        {KeyCode.LeftShift, "Left Shift"},
        {KeyCode.Return, "Enter"},
    };

    public static string GetVisualKeycode(KeyCode keyCode)
    {
        if (VisualKeycodes.ContainsKey(keyCode)) return VisualKeycodes[keyCode];
        else return keyCode.ToString();
    }
}
