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

        MapData.instance.mapTiles = new MapTile[mapSize.x, mapSize.y];
        MapData.instance.mapSize = mapSize;
        if(landscapeGenerator) yield return StartCoroutine(landscapeGenerator.MakeLandscape(mapSize));
        if(resourceGenerator) yield return StartCoroutine(resourceGenerator.GenerateResources(mapSize));
        PlayerSpawn.instance.SpawnPlayer();

        GameManager.instance.gameLoading = false;
        LoadingScreen.instance.ToggleLoadingScreen(false);
    }
}
