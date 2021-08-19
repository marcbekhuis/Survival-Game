using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Singleton<MapData>
{
    public MapTile[,] mapTiles;
    public Vector2Int mapSize;
    public List<BiomeCluster> biomeClusters = new List<BiomeCluster>();
    public Dictionary<Biome, List<MapTile>> mapTilesFilterByBiome = new Dictionary<Biome, List<MapTile>>();
    public Dictionary<Biome, List<MapTile>> mapTilesFilterByBiomeNoResource = new Dictionary<Biome, List<MapTile>>();
}
