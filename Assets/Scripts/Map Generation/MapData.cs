using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Singleton<MapData>
{
    public MapTile[,] grid;
    protected Vector2Int size;
    public float sizeMultiplier = 1;

    public List<BiomeCluster> biomeClusters = new List<BiomeCluster>();
    public Dictionary<Biome, List<MapTile>> mapTilesFilterByBiome = new Dictionary<Biome, List<MapTile>>();
    public Dictionary<Biome, List<MapTile>> mapTilesFilterByBiomeNoResource = new Dictionary<Biome, List<MapTile>>();

    public void SetupGrid(Vector2Int size)
    {
        this.size = size;
        grid = new MapTile[size.x, size.y];
    }

    public MapTile GetGridCell(Vector2Int gridPosition)
    {
        return grid[gridPosition.x, gridPosition.y];
    }

    public MapTile GetGridCell(int x, int y)
    {
        return grid[x, y];
    }

    public MapTile SetGridCell(int x, int y, MapTile mapTile)
    {
        grid[x, y] = mapTile;
        return grid[x, y];
    }

    public Vector2Int Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            sizeMultiplier = size.x * size.y / 10000f;
        }
    }
}
