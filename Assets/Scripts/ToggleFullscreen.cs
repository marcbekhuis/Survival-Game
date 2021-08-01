using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullscreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.GetKeyDown(InputActions.ToggleFullscreen))
        {
            if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) Screen.fullScreenMode = FullScreenMode.Windowed;
            else Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}
