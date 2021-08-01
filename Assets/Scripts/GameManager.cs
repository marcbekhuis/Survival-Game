using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameLoading = false;
    public bool gamePaused = false;
    public bool panelOpen = false;

    public bool GamePaused 
    {
        get => gameLoading || gamePaused;
    }

    public bool PlayerMovementDisabled
    {
        get => panelOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused) return;

        GameTime.time += Time.deltaTime;
    }
}
