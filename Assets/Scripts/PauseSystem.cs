using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : Singleton<PauseSystem>
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject[] closePanels = new GameObject[1];

    private void Update()
    {
        if (InputManager.instance.GetKeyDown(InputActions.Pause))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!pauseMenu.activeSelf)
        {
            foreach (var closePanel in closePanels)
            {
                if (!closePanel.activeSelf) continue;

                closePanel.SetActive(false);
                GameManager.instance.panelOpen = false;
                return;
            }
        }

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        GameManager.instance.gamePaused = pauseMenu.activeSelf;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        GameManager.instance.gamePaused = pauseMenu.activeSelf;
    }
}
