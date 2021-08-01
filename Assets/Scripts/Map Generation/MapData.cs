using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Singleton<MapData>
{
    public MapTile[,] mapTiles;
    public Vector2Int mapSize;
    public List<BiomeCluster> biomeClusters = new List<BiomeCluster>();
}
