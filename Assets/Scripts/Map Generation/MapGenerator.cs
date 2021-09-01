using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize = new Vector2Int(100, 100);

    [SerializeField] private LandscapeGenerator landscapeGenerator;
    [SerializeField] private ResourceGenerator resourceGenerator;

    private void Start()
    {
        StartCoroutine(GenerateMap());
    }

    IEnumerator GenerateMap()
    {
        LoadingScreen.instance.ToggleLoadingScreen(true);
        GameManager.instance.gameLoading = true;

        MapData.instance.SetupGrid(mapSize);
        if(landscapeGenerator) yield return StartCoroutine(landscapeGenerator.MakeLandscape());
        if(resourceGenerator) yield return StartCoroutine(resourceGenerator.GenerateResources(mapSize));
        if(PlayerSpawn.instance) PlayerSpawn.instance.SpawnPlayer();
        if(PathFinding.instance) PathFinding.instance.SetupPathFinding();

        GameManager.instance.gameLoading = false;
        LoadingScreen.instance.ToggleLoadingScreen(false);
    }
}
