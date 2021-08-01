using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    MapTile mapTile;

    public Biome biome {
        get
        {
            if (mapTile == null) return null;
            else return mapTile.biome;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int position = new Vector2Int(Mathf.FloorToInt(transform.position.x - 0.5f), Mathf.FloorToInt(transform.position.y - 0.5f));
        if (position.x < 0 || position.y < 0)
        {
            mapTile = null;
            return;
        }
        if (position.x >= MapData.instance.mapSize.x || position.y >= MapData.instance.mapSize.y)
        {
            mapTile = null;
            return;
        }
        mapTile = MapData.instance.mapTiles[position.x, position.y];
    }
}
